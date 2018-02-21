using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Stock
{
    public interface IPublisher
    {
        void Attach(ISubscriber sub);
        void Detach(ISubscriber sub);
        void Notify();
    }

    public class Stock : IPublisher
    {
        private readonly List<ISubscriber> _subscribers = new List<ISubscriber>();

        private Random _random;
        private Timer _timer = new Timer();

        public string Name { get; private set; }


        private float _value;
        public float Value
        {
            get => _value;
            private set
            {
                if (Value != value)
                {
                    _value = value;
                    Notify();
                }
            }
        }

        

        public Stock(float value, string name, Random random)
        {
            Name = name;
            Value = value;
            _random = random;

            // Initliazing the timer, such that the stock automatically updates randomly between every 2-5 seconds
            _timer.Interval = random.Next(2000,5000);
            _timer.Elapsed += (sender, args) =>
            {
                int rand = _random.Next(-1, 2);
                Value += Value * ((float) (_random.NextDouble() *5 *rand)/100);
            };
            _timer.Start();
        }


        public void Attach(ISubscriber sub) // Attach a subscriber, if not already a subscriber
        {
            foreach(var subscriber in _subscribers)
                if (subscriber == sub)
                    return;
            _subscribers.Add(sub);
        }

        public void Detach(ISubscriber sub) // Detach a subscriber
        {
            _subscribers.Remove(sub);
        }

        public void Notify()
        {
            foreach (var sub in _subscribers)
                sub.Update(this);
        }
    }
}
