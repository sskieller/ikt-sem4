using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartGrid.Controllers;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid
{
    public static class PowerDistributer
    {
        public static void HandleTransactions()
        {
            //Do all transactions here
            using (var prosumerDb = new SmartGridContext())
            {
                var uow = new UnitOfWork<Prosumer>(prosumerDb);
                var smartGridUow = new UnitOfWork<SmartGridModel>(prosumerDb);
                float netElectricity = 0;
                
                List<Prosumer> consumers = new List<Prosumer>();
                List<Prosumer> producers = new List<Prosumer>();
                List<Transaction> transactions = new List<Transaction>();

                //Go through all prosumers and get 
                foreach (var prosumer in uow.Repository.ReadAll())
                {
                    netElectricity += prosumer.Difference;
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

                DistributePowerBetweenMultiple(ref producers, ref consumers, ref transactions);


                if (netElectricity > 0)
                {
                    //Electricity in smart grid is positive, send to global grid
                    
                }
                else if (netElectricity < 0)
                {
                    //Electricity is negative, import power
                }

                TransactionController ctrl = new TransactionController();
                ctrl.Post(transactions).Wait(2000);

                uow.Commit(); //Commit at the end
            }
        }

        private static bool DistributePowerBetweenMultiple(ref List<Prosumer> producers, ref List<Prosumer> consumers, ref List<Transaction> transactions)
        {
            float pricePerKwh = 0;
            foreach (var producer in producers.ToArray())
            {
                float netImport = producer.Difference;

                //If the preferred producer is available in producers, choose that
                if (producers.Contains(producer.PreferedBuyer))
                {
                    if (DistributePowerBetweenSingle(producer, producer.PreferedBuyer, ref transactions, pricePerKwh) == 0)
                    {
                        consumers.Remove(producer.PreferedBuyer);
                    }
                    else
                    {
                        producers.Remove(producer);
                        continue;
                    }
                }

                while (producer.Remainder != 0 && consumers.Count > 0)
                {
                    if (DistributePowerBetweenSingle(producer, consumers[0], ref transactions, pricePerKwh) == 0)
                    {
                        consumers.Remove(producer.PreferedBuyer);
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
                float transferredAmount = consumer.Remainder;
                producer.Remainder -= consumer.Remainder;
                consumer.Remainder = 0;

                transactions.Add(new Transaction()
                {
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
                float transferredAmount = producer.Remainder;
                consumer.Remainder += producer.Remainder;
                producer.Remainder = 0;

                transactions.Add(new Transaction()
                {
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
    }

    
}