using System;

namespace SmartGridDemoProgram
{
    public static class ProsumerFactory
    {
        private static int _business = 0;
        private static int _houses = 0;
        public static Prosumer Create(string type = "")
        {
            if (type.ToLower() == "business")
                return new Prosumer("Business" + (++_business).ToString(), _business > 1 ? "Business" + (_business - 1).ToString() : String.Empty, "Business");

            return new Prosumer("House" + (++_houses).ToString(), _houses > 1 ? "House" + (_houses - 1).ToString() : String.Empty, "House");
        }
    }

    public class Prosumer
    {
        private readonly float _stdDiviation = 2f;
        private readonly int _minKwh = 8;
        private readonly int _maxKwh = 16;
        private readonly Random _rnd;

        public string Name { get; set; }
        public string PreferedBuyer { get; set; }
        public string Type { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }


        public Prosumer(string name, string preferedBuyer, string type)
        {
            Name = name;
            PreferedBuyer = preferedBuyer;
            Type = type;

            _rnd = new Random(name.GetHashCode()); // Ensures data stays the same every time it runs. Makes it easier to debug

            if (Type == "Business")
            {
                _maxKwh *= 5;
                _minKwh *= 5;
                _stdDiviation *= 2.5f;
            }


            Produced = _rnd.Next(_minKwh, _maxKwh) + (float)(_rnd.NextDouble() * _stdDiviation);
            Consumed = _rnd.Next(_minKwh, _maxKwh) + (float)(_rnd.NextDouble() * _stdDiviation);
        }

        public void Update()
        {

            // Updating Produced
            if (_rnd.NextDouble() > 0.5)
            {
                // Positive change
                Produced += (float) (_rnd.NextDouble() * _stdDiviation);
            }
            else
            {
                Produced -= (float)(_rnd.NextDouble() * _stdDiviation);
            }

            // Updating Consumed
            if (_rnd.NextDouble() > 0.5)
            {
                // Positive change
                Consumed += (float)(_rnd.NextDouble() * _stdDiviation);
            }
            else
            {
                // Negative change
                Consumed -= (float)(_rnd.NextDouble() * _stdDiviation);
            }

            if (Type == "House")
            {
                if (Produced > 30f) Produced = 30f;
                if (Consumed < 3f) Consumed = 3f;
                if (Consumed > 30f) Consumed = 30f;
                if (Produced < 3f) Produced = 3f;
            }
            else // Business
            {
                if (Produced > 150f) Produced = 150f;
                if (Consumed < 20f) Consumed = 20f;
                if (Consumed > 150f) Consumed = 150f;
                if (Produced < 20f) Produced = 20f;
            }
        }

        public override string ToString()
        {
            return Name + "Produced: " + Produced + " -- Consumed: " + Consumed;
        }
    }
}