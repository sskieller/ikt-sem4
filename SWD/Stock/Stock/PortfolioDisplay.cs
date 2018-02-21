using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
    public class PortfolioDisplay
    {
        public void Display(List<Tuple<uint, Stock>> stocks)
        {
            foreach(var stock in stocks)
                Console.WriteLine("Stock: {0,-12} | StockValue: {3,-10} | Amount: {1,-5} | Total Value: {2,-12}", 
                                  stock.Item2.Name, stock.Item1, stock.Item2.Value*stock.Item1, stock.Item2.Value);
        }
    }
}
