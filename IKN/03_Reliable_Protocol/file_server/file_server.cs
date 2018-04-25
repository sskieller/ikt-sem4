using System;
using System.IO;
using System.Text;
using Transportlaget;
using Library;

namespace Application
{
	class file_server
	{
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		private const int BUFSIZE = 1000;
		private const string APP = "FILE_SERVER";

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// </summary>
		private file_server ()
		{
			Transport transport = new Transport(BUFSIZE, APP);
			byte[] buffer = new byte[BUFSIZE];
			Console.WriteLine("Server ready");
			Console.WriteLine();
			while (true)
			{
				Console.WriteLine("Waiting for file request");

				int receivedBytes = transport.receive(ref buffer);
				string filename = Encoding.ASCII.GetString(buffer);
				Console.WriteLine("Received: {0}", filename);

				long fileSize = LIB.check_File_Exists(LIB.extractFileName(filename));

				if (fileSize == 0)
				{
					Console.WriteLine("File did not exist");
					string nul = "0";
					transport.send(Encoding.ASCII.GetBytes(nul), nul.Length);
				}
				else
				{
					Console.WriteLine("File \"{0}\" is {1} bytes long, now sending file length to client", LIB.extractFileName(filename), fileSize);

					//Send file size
					transport.send(Encoding.ASCII.GetBytes(fileSize.ToString()), Encoding.ASCII.GetBytes(fileSize.ToString()).Length);

					var file = File.OpenRead(LIB.extractFileName(filename));
					int remainingBytes = (int)fileSize;
					while (remainingBytes > 0)
					{
						remainingBytes = file.Read(buffer, 0, BUFSIZE);
						transport.send(buffer, remainingBytes);
					}

					Console.WriteLine("File \"{0}\" sent", LIB.extractFileName(filename));
				}


			}
		}



		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			new file_server();
		}
	}
}