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
				string filename = LIB.extractFileName(Encoding.ASCII.GetString(buffer, 0, receivedBytes));
				Console.WriteLine("Received: \"{0}\"", filename);


				if (File.Exists(filename) == false)
				{
					
					Console.WriteLine("File did not exist");
					Console.WriteLine ();
					string nul = "0";
					transport.send(Encoding.ASCII.GetBytes(nul), nul.Length);
				}
				else
				{
					Console.WriteLine ("Found file");
					var file = File.OpenRead(LIB.extractFileName(filename));

					byte[] fileSize = Encoding.ASCII.GetBytes(file.Length.ToString());
					Console.WriteLine("File \"{0}\" exists with size {1}, now sending file length to client", LIB.extractFileName(filename), Encoding.ASCII.GetString(fileSize));

					//Send file size
					transport.send(fileSize, fileSize.Length);
				
					int remainingBytes = file.Read(buffer,0, BUFSIZE);
					int remaining = int.Parse(file.Length.ToString ());
					while (remainingBytes > 0) 
					{
						transport.send (buffer, remainingBytes);
						remainingBytes = file.Read (buffer, 0, BUFSIZE);
						remaining = remaining - remainingBytes;
						Console.WriteLine ("Need to write {0} of {1} bytes", remaining, file.Length.ToString ());
					}
					file.Close ();

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