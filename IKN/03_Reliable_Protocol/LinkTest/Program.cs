using System;
using Linklaget;
using System.Collections.Generic;

namespace LinkTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 1) 
			{
				Console.WriteLine ("Please enter <send> or not");
				return;
			}

			Link link = new Link (1000, "empty");

			byte[] buffer = new byte[255];

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

			if (args [0] == "send") 
			{
				Console.WriteLine ("Send passed as argument to application: sending 'HELLO ABCD' to serial");
				link.send (buffer, 10);
			}

			for (int i = 0; i < buffer.Length; ++i) 
			{
				buffer [i] = (byte)'\0'; //Fill buffer with null
			}

			int length = link.receive (ref buffer);


			Console.WriteLine ();
			Console.Write ("Received data: ");
			for (int i = 0; i < length; ++i) 
			{
				Console.Write ((char)buffer [i]);
			}
			Console.WriteLine ();



		}
	}
}
