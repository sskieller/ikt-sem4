using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FWPS
{
    /////////////////////////////////////////////////
    /// Interface for Debug Writer
    /////////////////////////////////////////////////
    interface IDebugWriter
    {
        void Write(string text);
        void Clear();
        string Read { get; }
    }
    /////////////////////////////////////////////////
    /// Stub class for Debug Writer
    /////////////////////////////////////////////////
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

    /////////////////////////////////////////////////
    /// Exception thrown for debug writer mock when clearing
    /////////////////////////////////////////////////
    public class ClearExecutedException : Exception
    {
    }

    /////////////////////////////////////////////////
    /// Exception thrown for debug writer mock when writing
    /////////////////////////////////////////////////
    public class WriteExecutedException : Exception
    {
    }

    /////////////////////////////////////////////////
    /// Debug Log class
    /////////////////////////////////////////////////
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

        /////////////////////////////////////////////////
        /// Public method for clearing Debug Log
        /////////////////////////////////////////////////
        public void Clear() => clear();
        /////////////////////////////////////////////////
        /// Private implementation from Clear() for clearing Debug Log
        /////////////////////////////////////////////////
        private static void clear() => File.Open("out.txt", FileMode.Create).Close();


        /////////////////////////////////////////////////
        /// Public method for reading Debug Log
        /////////////////////////////////////////////////
        public string Read => read;

        /////////////////////////////////////////////////
        /// Private implementation for Read() for reading Debug Log
        /////////////////////////////////////////////////
        private static string read => File.ReadAllText("out.txt");
        
    }

}


