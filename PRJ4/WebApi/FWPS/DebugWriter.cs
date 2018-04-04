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
	    //static StreamWriter writer = new StreamWriter(fs);

	    public static void Write(string text)
	    {
		    fs = System.IO.File.Open("out.txt", FileMode.Append);

			//fs.Write(Decoder .UTF8.GetBytes(text));

		    byte[] bytestowrite = System.Text.Encoding.UTF8.GetBytes(text);

			fs.Write(bytestowrite, 0, bytestowrite.Length);

			fs.Close();
	    }	
	    
    }
}
