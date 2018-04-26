using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FWPS
{
    public static class DebugWriter
    {
		static FileStream fs;

	    public static void Write(string text)
	    {
		    fs = System.IO.File.Open("out.txt", FileMode.Append);

			//fs.Write(Decoder .UTF8.GetBytes(text));

		    byte[] bytestowrite = System.Text.Encoding.UTF8.GetBytes(text + "\r\n");

			fs.Write(bytestowrite, 0, bytestowrite.Length);

			fs.Close();
	    }
        public static void Clear()
        {
            File.Open("out.txt", FileMode.Create).Close();
        }

        public static string Read()
        {
            return File.ReadAllText("out.txt");
        }
    }

}


