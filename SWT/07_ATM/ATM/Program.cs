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

            var dt = dp.ParseTime("20180405113000123");

            

            while (Console.Read() != 'q') ;
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
