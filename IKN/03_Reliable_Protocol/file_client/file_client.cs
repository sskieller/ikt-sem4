using System;
using System.IO;
using System.Text;
using Transportlaget;
using Library;

namespace Application
{
	class file_client
	{
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		private const int BUFSIZE = 1000;
		private const string APP = "FILE_CLIENT";

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// 
		/// file_client metoden opretter en peer-to-peer forbindelse
		/// Sender en forspÃ¸rgsel for en bestemt fil om denne findes pÃ¥ serveren
		/// Modtager filen hvis denne findes eller en besked om at den ikke findes (jvf. protokol beskrivelse)
		/// Lukker alle streams og den modtagede fil
		/// Udskriver en fejl-meddelelse hvis ikke antal argumenter er rigtige
		/// </summary>
		/// <param name='args'>
		/// Filnavn med evtuelle sti.
		/// </param>
		private file_client(String[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Please enter a filename");
				return;
			}
			Transport transport = new Transport(BUFSIZE, APP);
			byte[] receiveBuffer = new byte[BUFSIZE];
			long fileSize = 0;


			string filename = args[0];

			Console.WriteLine("Gettting file with name: {0}", filename);

			transport.send(Encoding.ASCII.GetBytes(filename), filename.Length);


			transport.receive(ref receiveBuffer);
			string tempFileSize = Encoding.ASCII.GetString (receiveBuffer);

			Console.WriteLine ("Got file size: {0}", tempFileSize);

			if (long.TryParse(tempFileSize, out fileSize) == false) 
			{
				Console.WriteLine("Filesize was not returned, actually received: {0}", Encoding.ASCII.GetString(receiveBuffer));
				return;
			}

			if (fileSize == 0)
			{
				Console.WriteLine("File does not exist, filesize is 0");
				return;
			}

			Console.WriteLine("Total file size from server: {0} bytes", fileSize);

			var file = File.Create(filename); //Create new file
			int received = 0;
			while (file.Position < fileSize) //Still need to receive more data
			{
				received = transport.receive(ref receiveBuffer);
				file.Write(receiveBuffer, 0, received);
				Console.WriteLine("{0} bytes of {1} written so far", file.Position, fileSize);
			}
			file.Close();

			Console.WriteLine("Transfer complete");
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// First argument: Filname
		/// </param>
		public static void Main (string[] args)
		{
			new file_client(args);
		}
	}
}