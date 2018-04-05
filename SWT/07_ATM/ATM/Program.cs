using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            //var atm = new ATM();

            TransponderDataParser dp = new TransponderDataParser();

            DateTime dt;
            string tag;
            int x;
            int y;
            uint alt;

            dp.ParseData("ATR423;40000;10000;14000;20180405113000123", out tag, out x, out y, out alt, out dt);

            Console.WriteLine(tag);
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(alt);
            Console.WriteLine(dt);

            //while (Console.Read() != 'q') ;
        }
    }

    public class ATM
    {
        private readonly ITransponderReceiver _receiver;
        public ATM()
        {
            _receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            _receiver.TransponderDataReady += ReceiverOnTransponderDataReady;
        }

        private void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs rawTransponderDataEventArgs)
        {
            foreach(var data in rawTransponderDataEventArgs.TransponderData)
                Console.WriteLine(data);
            //Console.WriteLine(rawTransponderDataEventArgs.TransponderData);
        }
    }

    
}
