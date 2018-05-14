using System;
using System.IO.Ports;
using System.Collections.Generic;

/// <summary>
/// Link.
/// </summary>
namespace Linklaget
{
	/// <summary>
	/// Link.
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The DELIMITE for slip protocol.
		/// </summary>
		const byte DELIMITER = (byte)'A';
		/// <summary>
		/// The buffer for link.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The serial port.
		/// </summary>
		SerialPort serialPort;

		/// <summary>
		/// Initializes a new instance of the <see cref="link"/> class.
		/// </summary>
		public Link (int BUFSIZE, string APP)
		{

			serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);


			if(!serialPort.IsOpen)
				serialPort.Open();

			buffer = new byte[(BUFSIZE*2)];

			// Uncomment the next line to use timeout
			//serialPort.ReadTimeout = 500;

			serialPort.DiscardInBuffer ();
			serialPort.DiscardOutBuffer ();
		}

		/// <summary>
		/// Send the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send (byte[] buf, int size)
		{
			List<byte> frame = new List<byte>();
			frame.Add(DELIMITER); //Add delimiter to start of frame


			for (int i = 0; i < size; ++i)
			{
				if (buf[i] == DELIMITER)
				{
					//If an 'A' is hit, add 'BC' instead
					frame.Add((byte)'B');
					frame.Add((byte)'C');
				}
				else
				{
					//Add frame byte
					frame.Add(buf[i]);

					//If 'B' is hit, add an additional 'D'
					if (buf[i] == (byte) 'B')
						frame.Add((byte) 'D');
				}
			}

			//Add delimiter to end of frame
			frame.Add(DELIMITER);

			//Write to serial port
			serialPort.Write(frame.ToArray(), 0, frame.Count); 
		}

		/// <summary>
		/// Receive the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public int receive (ref byte[] buf)
		{
			byte tempByte = (byte) '0';
			while (tempByte != DELIMITER)
			{
				//Read single byte until delimiter is read
				tempByte = (byte) serialPort.ReadByte();
			}
			int count = 0;
			tempByte = (byte) serialPort.ReadByte();

			while (tempByte != DELIMITER) //Keep on reading till delimiter is reached
			{
				if (tempByte == (byte) 'B')
				{
					
					//May be either 'A' or 'B'
					tempByte = (byte) serialPort.ReadByte();

					if (tempByte == (byte) 'C')
						buf[count] = (byte) 'A';
					else
						buf[count] = (byte) 'B';
				}
				else
				{
					buf[count] = tempByte;
				}

				++count; //Increment count by 1
				tempByte = (byte) serialPort.ReadByte(); //Read next byte
 			}

			return count; //Return amount of bytes read
		}
	}
}
