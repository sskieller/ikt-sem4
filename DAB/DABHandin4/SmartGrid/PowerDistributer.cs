using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SmartGrid.Controllers;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid
{
    public static class PowerDistributer
    {
        public static async Task HandleTransactions()
        {
            //Do all transactions here
            using (var prosumerDb = new SmartGridContext())
            {
                var uow = new UnitOfWork<Prosumer>(prosumerDb);
                var smartGridUow = new UnitOfWork<SmartGridModel>(prosumerDb);
                float netElectricity = 0;
                float produced = 0;
                float consumed = 0;

                List<Prosumer> consumers = new List<Prosumer>();
                List<Prosumer> producers = new List<Prosumer>();
                List<Transaction> transactions = new List<Transaction>();

                //Go through all prosumers and get 
                foreach (var prosumer in uow.Repository.ReadAll())
                {
                    if (prosumer.Type == "Grid")
                        continue;
                    netElectricity += prosumer.Difference;
                    consumed += prosumer.Consumed;
                    produced += prosumer.Produced;
                    if (prosumer.Difference > 0)
                    {
                        //Add to list of producers if more electricity is produced than consumed
                        producers.Add(prosumer);
                    }
                    else if (prosumer.Difference < 0)
                    {
                        //Add to list of consumres if more electricity is consumed rather than produced
                        consumers.Add(prosumer);
                    }
                    //Otherwise do not add to either consumers or producers
                }

                //CreateGridProsumer(ref producers, ref consumers, netElectricity);
                bool profit = DistributePowerBetweenMultiple(ref producers, ref consumers, ref transactions);

                Prosumer smartGrid = null;
                Prosumer nationalGrid = null;


                if (profit)
                {
                    smartGrid = new Prosumer()
                    {
                        Name = "SmartGrid",
                        PreferedBuyerName = "NationalGrid",
                        Type = "Grid",
                        Produced = netElectricity,
                        Consumed = 0,
                        Difference = netElectricity,
                        Remainder = netElectricity
                    };
                    nationalGrid = new Prosumer()
                    {
                        Name = "NationalGrid",
                        PreferedBuyerName = "SmartGrid",
                        Type = "Grid",
                        Produced = 0,
                        Consumed = netElectricity,
                        Difference = 0 - netElectricity,
                        Remainder = 0 - netElectricity,
                    };

                    //More power is produced than consumed, sell the rest to SmartGrid and then National Grid
                    SellPowerToSmartGrid(ref producers, ref transactions, smartGrid);

                    producers.Add(smartGrid);
                    consumers.Add(nationalGrid);
                    DistributePowerBetweenMultiple(ref producers, ref consumers, ref transactions);


                }
                else
                {
                    netElectricity = Math.Abs(netElectricity);
                    float price = BitcoinPriceGetter.GetPrice();
                    smartGrid = new Prosumer()
                    {
                        Name = "SmartGrid",
                        PreferedBuyerName = "NationalGrid",
                        Type = "Grid",
                        Consumed = netElectricity,
                        Produced = 0,
                        Difference = 0 - netElectricity,
                        Remainder = 0 - netElectricity
                    };
                    nationalGrid = new Prosumer()
                    {
                        Name = "NationalGrid",
                        PreferedBuyerName = "SmartGrid",
                        Type = "Grid",
                        Consumed = 0,
                        Produced = netElectricity,
                        Difference = netElectricity,
                        Remainder = netElectricity,
                    };

                    //Save transaction
                    transactions.Add(new Transaction()
                    {
                        Id = string.Format("{0}-{1}-{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month,
                            DateTime.Now.Year, nationalGrid.Name, smartGrid.Name),
                        Consumer = smartGrid.Name,
                        Producer = nationalGrid.Name,
                        TransactionDate = DateTime.Now,
                        KwhAmount = nationalGrid.Produced,
                        PricePerKwh = price,
                        TotalPrice = nationalGrid.Produced * price
                    });

                    smartGrid.Remainder = netElectricity;
                    nationalGrid.Remainder = 0;
                    
                    producers.Add(smartGrid);
                    DistributePowerBetweenMultiple(ref producers, ref consumers, ref transactions);
                    //More power is consumed, buy the rest from National grid
                }

                SmartGridModelsController smartCtrl = new SmartGridModelsController();
                smartCtrl.PostSmartGridModel(new SmartGridModel()
                {
                    TimeStamp = DateTime.Now,
                    TotalGeneration = produced,
                    TotalForbrug = consumed,
                    Brutto = produced - consumed,
                    
                });


                ProsumersController pCtrl = new ProsumersController();
                
                pCtrl.UpdateGrids(new ProsumerDTO[]
                {
                    new ProsumerDTO(smartGrid),
                    new ProsumerDTO(nationalGrid)
                });

                TransactionController ctrl = new TransactionController();
                await ctrl.Post(transactions);

                uow.Commit(); //Commit at the end
            }
        }

        private static void SellPowerToSmartGrid(ref List<Prosumer> producers, ref List<Transaction> transactions, Prosumer grid)
        {
            float pricePerKwh = BitcoinPriceGetter.GetPrice(); //EDIT HERE
            float sold = 0;
            foreach (Prosumer producer in producers)
            {
                sold += producer.Remainder;

                //Save transaction
                transactions.Add(new Transaction()
                {
                    Id = string.Format("{0}-{1}-{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month,
                        DateTime.Now.Year, producer.Name, grid.Name),
                    Consumer = grid.Name,
                    Producer = producer.Name,
                    TransactionDate = DateTime.Now,
                    KwhAmount = producer.Remainder,
                    PricePerKwh = pricePerKwh,
                    TotalPrice = producer.Remainder * pricePerKwh
                });


                producer.Remainder = 0;
            }
            producers.Clear();
        }


        private static bool DistributePowerBetweenMultiple(ref List<Prosumer> producers, ref List<Prosumer> consumers, ref List<Transaction> transactions)
        {
            float pricePerKwh = BitcoinPriceGetter.GetPrice(); //EDIT HERE
            foreach (var producer in producers.ToArray())
            {
                float netImport = producer.Remainder;
                Prosumer preferredConsumer = consumers.Find(x => x.Name == producer.PreferedBuyerName);
                //If the preferred producer is available in producers, choose that
                if (preferredConsumer != null)
                {
                    if (DistributePowerBetweenSingle(producer, preferredConsumer, ref transactions, pricePerKwh) == 0)
                    {
                        consumers.Remove(preferredConsumer);
                    }
                    else
                    {
                        producers.Remove(producer);
                        continue;
                    }
                }

                if (consumers.Count == 0)
                    return true; //No more consumers for power

                while (producer.Remainder != 0 && consumers.Count > 0)
                {
                    if (DistributePowerBetweenSingle(producer, consumers[0], ref transactions, pricePerKwh) == 0)
                    {
                        consumers.Remove(consumers[0]);
                    }
                    else
                    {
                        producers.Remove(producer);
                        break;
                    }
                    if (consumers.Count == 0)
                        return true; //No more consumers for power
                }
            }

            return false; //Not enough producers, we need to import power
        }
        /// <summary>Distributes power from producer to consumer, returns 0 on comsumer not having any remainder and 1 on producer not having any remainder
        /// </summary>
        private static int DistributePowerBetweenSingle(Prosumer producer, Prosumer consumer, ref List<Transaction> transactions, float pricePerKwh)
        {
            if (Math.Abs(producer.Remainder) > Math.Abs(consumer.Remainder))
            {
                //More power has been produced than consumed
                float transferredAmount = Math.Abs(consumer.Remainder);
                producer.Remainder += consumer.Remainder;
                consumer.Remainder = 0;

                transactions.Add(new Transaction()
                {
                    Id = string.Format("{0}-{1}-{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month,
                        DateTime.Now.Year, producer.Name, consumer.Name) ,
                    Consumer = consumer.Name,
                    Producer = producer.Name,
                    TransactionDate = DateTime.Now,
                    KwhAmount = transferredAmount,
                    PricePerKwh = pricePerKwh,
                    TotalPrice = transferredAmount * pricePerKwh
                });

                return 0;
            }
            else
            {
                //More power has been consumed than produced
                float transferredAmount = Math.Abs(producer.Remainder);
                consumer.Remainder += producer.Remainder;
                producer.Remainder = 0;

                transactions.Add(new Transaction()
                {
                    Id = string.Format("{0}-{1}-{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month,
                        DateTime.Now.Year, producer.Name, consumer.Name),
                    Consumer = consumer.Name,
                    Producer = producer.Name,
                    TransactionDate = DateTime.Now,
                    KwhAmount = transferredAmount,
                    PricePerKwh = pricePerKwh,
                    TotalPrice = transferredAmount * pricePerKwh
                });

                return 1;
            }
        }

        private static void CreateGridProsumer(ref List<Prosumer> producers, ref List<Prosumer> consumers, float netElectricity)
        {

            if (netElectricity > 0)
            {
                //Create smartGrid is a producer, nationalGrid is a consumer
                Prosumer smartGrid = new Prosumer()
                {
                    Consumed = 0,
                    Name = "SmartGrid",
                    PreferedBuyerName = "",
                    Produced = netElectricity,
                    Difference = netElectricity,
                    Remainder = 0
                };
                Prosumer nationalGrid = new Prosumer()
                {
                    Consumed = netElectricity,
                    Name = "NationalGrid",
                    PreferedBuyerName = "SmartGrid",
                    Produced = 0,
                    Difference = 0 - netElectricity,
                    Remainder = 0 - netElectricity
                };
                //Electricity in smart grid is positive, send to global grid
                ProsumersController pCtrl = new ProsumersController();
                
                pCtrl.UpdateGrids(new ProsumerDTO[]
                {
                    new ProsumerDTO(smartGrid),
                    new ProsumerDTO(nationalGrid)
                });
                
                consumers.Add(nationalGrid);

            }
            else if (netElectricity < 0)
            {
                //Electricity is negative, import power
                Prosumer smartGrid = new Prosumer()
                {
                    Consumed = netElectricity,
                    Name = "SmartGrid",
                    PreferedBuyerName = "NationalGrid",
                    Produced = 0,
                    Difference = 0 - netElectricity,
                    Remainder = 0
                };
                Prosumer nationalGrid = new Prosumer()
                {
                    Consumed = 0,
                    Name = "NationalGrid",
                    PreferedBuyerName = "",
                    Produced = netElectricity,
                    Difference = netElectricity,
                    Remainder = netElectricity
                };
                //Electricity in smart grid is positive, send to global grid
                ProsumersController pCtrl = new ProsumersController();
                
                pCtrl.UpdateGrids(new ProsumerDTO[]
                {
                    new ProsumerDTO(smartGrid),
                    new ProsumerDTO(nationalGrid)
                });

                producers.Add(nationalGrid);
            }
        }
    }



}