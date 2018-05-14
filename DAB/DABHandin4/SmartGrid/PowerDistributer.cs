using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

                foreach (var consumer in consumers)
                {

                }

                if (netElectricity != 0)
                {
                    //Electricity in smart grid is either net negative or net positive
                }

                uow.Commit(); //Commit at the end
            }
        }

        private static void DistributePower(Prosumer producer, Prosumer consumer)
        {

        }
    }

    
}