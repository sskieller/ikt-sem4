using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
    public interface ISubscriber
    {
        void Update(IPublisher pub);
    }



    public class Portfolio : ISubscriber
    {
        public List<Tuple<uint, Stock>> Stocks = new List<Tuple<uint, Stock>>();
        private readonly PortfolioDisplay _display;

        public Portfolio(PortfolioDisplay display)
        {
            _display = display;
        }

        public void Update(IPublisher pub)
        {
            _display.Display(Stocks);
        }

        public void AddStock(Stock stock, uint amount)
        {
            stock.Attach(this);

            // Checking if the portfolio already contains the stock, and if so, increments the amount of stock held.
            for (var i = 0; i < Stocks.Count; i++)
            {
                if (Stocks[i].Item2 == stock)
                {
                    amount += Stocks[i].Item1;
                    Stocks.RemoveAt(i);
                    Stocks.Add(Tuple.Create(amount, stock));
                    return;
                }
            }

            // Adds the stock since we dont already own some
            Stocks.Add(Tuple.Create(amount, stock));
        }



        public void RemoveStock(Stock stock) // Removes all stocks from portfolio
        {
            stock.Detach(this);

            for (var i = 0; i < Stocks.Count; i++)
                if (Stocks[i].Item2 == stock)
                    Stocks.RemoveAt(i);

        }
    }
}
