using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartGrid.Data
{
    public class Prosumer
    {
        private static float _stdDiviation = 5f;
        private static int _minKWH = 8;
        private static int _maxKWH = 16;


        public string Name { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }


        public Prosumer(string name)
        {
            Name = name;

            var rnd = new Random(name.GetHashCode());

            Produced = rnd.Next(_minKWH, _maxKWH) + (float)(rnd.NextDouble() * _stdDiviation);
            Consumed = rnd.Next(_minKWH, _maxKWH) + (float)(rnd.NextDouble() * _stdDiviation);
        }

        public void Update()
        {
            var rnd = new Random();


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
        }
    }
}