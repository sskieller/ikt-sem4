using System;

namespace ATM
{
    internal interface ITransponderDataParser
    {
        DateTime ParseTime(string time);

        void ParseData(string inputString, out string tag, out int Xcoord, out int Ycoord, out uint altitude,
            out DateTime time);
    }

    public class TransponderDataParser : ITransponderDataParser
    {
        public DateTime ParseTime(string time)
        {
            int year = int.Parse(time.Substring(0, 4));
            int month = int.Parse(time.Substring(4, 2));
            int day = int.Parse(time.Substring(6, 2));
            int hour = int.Parse(time.Substring(8, 2));
            int min = int.Parse(time.Substring(10, 2));
            int sec = int.Parse(time.Substring(12, 2));
            int millis = int.Parse(time.Substring(14, 3));

            return new DateTime(year, month, day, hour, min, sec, millis);
        }

        public void ParseData(string inputString, out string tag, out int Xcoord, out int Ycoord, out uint altitude,
            out DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}