using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FWPS
{
    interface IDebugWriter
    {
        void Write(string text);
        void Clear();
        string Read { get; }
    }

    public class StubDebugWriter : IDebugWriter
    {
        public void Write(string text)
        {
            throw new WriteExecutedException();
        }

        public void Clear()
        {
            throw new ClearExecutedException();
        }

        public string Read { get; }
    }

    public class ClearExecutedException : Exception
    {
    }

    public class WriteExecutedException : Exception
    {
    }

    public class DebugWriter : IDebugWriter
    {
		static FileStream fs;
        // Public method for interface
        public void Write(string text) => write(text);

        // Private static implementation
        private static void write(string text)
        {
            fs = System.IO.File.Open("out.txt", FileMode.Append);

            //fs.Write(Decoder .UTF8.GetBytes(text));

            byte[] bytestowrite = System.Text.Encoding.UTF8.GetBytes(text + "\r\n");

            fs.Write(bytestowrite, 0, bytestowrite.Length);

            fs.Close();
        }

        //public void Write(string text)
        //{
        //    fs = System.IO.File.Open("out.txt", FileMode.Append);

        //    //fs.Write(Decoder .UTF8.GetBytes(text));

        //    byte[] bytestowrite = System.Text.Encoding.UTF8.GetBytes(text + "\r\n");

        //    fs.Write(bytestowrite, 0, bytestowrite.Length);

        //    fs.Close();
        //}

        // Public method for interface
        public void Clear() => clear();
        // Private static implementation
        private static void clear() => File.Open("out.txt", FileMode.Create).Close();

        //public void Clear()
        //{
        //    File.Open("out.txt", FileMode.Create).Close();
        //}

        // Public method for interface
        public string Read => read;
        // Private static implementation
        private static string read => File.ReadAllText("out.txt");

        //public string Read()
        //{
        //    return File.ReadAllText("out.txt");
        //}
    }

}


