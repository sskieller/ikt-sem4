using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock;

namespace StockApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            var microsoft = new Stock.Stock(100, "Microsoft", random);
            var google = new Stock.Stock(150, "Google", random);
            var vestas = new Stock.Stock(332, "Vestas", random);

            var portfolio = new Portfolio(new PortfolioDisplay());

            portfolio.AddStock(microsoft, 50);
            portfolio.AddStock(microsoft, 150); //20000
            portfolio.AddStock(google, 120);    //18000
            portfolio.AddStock(vestas, 11);     //3652

            //portfolio.RemoveStock(google); // Removes the google stock again

            while (Console.ReadKey().KeyChar != 'q') // Press q to exit
            {

            }

        }
    }
}
