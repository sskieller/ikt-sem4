using System;
using Linklaget;
using System.Collections.Generic;

namespace LinkTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 2) 
			{
				Console.WriteLine ("Please enter <send> or not");
				return;
			}

			Link link = new Link (1000, "empty");

			byte[] buffer = new byte[10];

			buffer [0] = (byte)'H';
			buffer [1] = (byte)'E';
			buffer [2] = (byte)'L';
			buffer [3] = (byte)'L';
			buffer [4] = (byte)'O';

			buffer [5] = (byte)' ';

			buffer [6] = (byte)'A';
			buffer [7] = (byte)'B';
			buffer [8] = (byte)'C';
			buffer [9] = (byte)'D';

			if (args [1] == "send") 
			{
				link.send (buffer, 10);
			}

			link.receive (ref buffer);

			Console.WriteLine (buffer);


		}
	}
}
