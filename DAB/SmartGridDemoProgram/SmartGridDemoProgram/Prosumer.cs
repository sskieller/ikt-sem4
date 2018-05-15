using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartGrid.Data
{
    public class Prosumer
    {
        private static float _stdDiviation = 2f;
        private static int _minKWH = 8;
        private static int _maxKWH = 16;
        private Random rnd;

        public string Name { get; set; }
        public string PreferedBuyer { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }


        public Prosumer(string name, string preferedBuyer)
        {
            Name = name;
            PreferedBuyer = preferedBuyer;

            rnd = new Random(name.GetHashCode());

            Produced = rnd.Next(_minKWH, _maxKWH) + (float)(rnd.NextDouble() * _stdDiviation);
            Consumed = rnd.Next(_minKWH, _maxKWH) + (float)(rnd.NextDouble() * _stdDiviation);
        }

        public void Update()
        {

            // Updating Produced
            if (rnd.NextDouble() > 0.5)
            {
                // Positive change
                Produced += (float) (rnd.NextDouble() * _stdDiviation);
            }
            else
            {
                Produced -= (float)(rnd.NextDouble() * _stdDiviation);
            }

            // Updating Consumed
            if (rnd.NextDouble() > 0.5)
            {
                // Positive change
                Consumed += (float)(rnd.NextDouble() * _stdDiviation);
            }
            else
            {
                // Negative change
                Consumed -= (float)(rnd.NextDouble() * _stdDiviation);
            }

            if (Produced > 30f) Produced = 30f;
            if (Consumed < 0f) Consumed = 0f;
        }
    }
}