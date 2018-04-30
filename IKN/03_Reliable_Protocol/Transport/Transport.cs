using System;
using Linklaget;

/// <summary>
/// Transport.
/// </summary>
namespace Transportlaget
{
	/// <summary>
	/// Transport.
	/// </summary>
	public class Transport
	{
		/// <summary>
		/// The link.
		/// </summary>
		private Link link;
		/// <summary>
		/// The 1' complements checksum.
		/// </summary>
		private Checksum checksum;
		/// <summary>
		/// The buffer.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The seq no.
		/// </summary>
		private byte seqNo;
		/// <summary>
		/// The old_seq no.
		/// </summary>
		private byte old_seqNo;
		/// <summary>
		/// The error count.
		/// </summary>
		private int errorCount;
		/// <summary>
		/// The DEFAULT_SEQNO.
		/// </summary>
		private const int DEFAULT_SEQNO = 2;
		/// <summary>
		/// The data received. True = received data in receiveAck, False = not received data in receiveAck
		/// </summary>
		private bool dataReceived;
		/// <summary>
		/// The number of data the recveived.
		/// </summary>
		private int recvSize = 0;
		/// <summary>
		/// Generate noise every N bytes
		/// </summary>
		public int GenerateNoiseEvery {get;set;} = 0;
		/// <summary>
		/// counter for noise generation
		/// </summary>
		private int noiseCounter = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Transport"/> class.
		/// </summary>
		public Transport (int BUFSIZE, string APP)
		{
			link = new Link(BUFSIZE+(int)TransSize.ACKSIZE, APP);
			checksum = new Checksum();
			buffer = new byte[BUFSIZE+(int)TransSize.ACKSIZE];
			seqNo = 0;
			old_seqNo = DEFAULT_SEQNO;
			errorCount = 0;
			dataReceived = false;
		}

		/// <summary>
		/// Receives the ack.
		/// </summary>
		/// <returns>
		/// The ack.
		/// </returns>
		private bool receiveAck()
		{
			recvSize = link.receive(ref buffer);
			dataReceived = true;

			if (recvSize == (int)TransSize.ACKSIZE) {
				dataReceived = false;
				if (!checksum.checkChecksum (buffer, (int)TransSize.ACKSIZE) ||
					buffer [(int)TransCHKSUM.SEQNO] != seqNo ||
					buffer [(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
				{
					seqNo = (byte) buffer[(int)TransCHKSUM.SEQNO];
					return false;
				}
				else
				{
					seqNo = (byte)((buffer[(int)TransCHKSUM.SEQNO] + 1) % 2);
					return true;
				}
			}

			return false;

		}

		/// <summary>
		/// Sends the ack.
		/// </summary>
		/// <param name='ackType'>
		/// Ack type.
		/// </param>
		private void sendAck (bool ackType)
		{
			byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
			ackBuf [(int)TransCHKSUM.SEQNO] = (byte)
				(ackType ? (byte)buffer [(int)TransCHKSUM.SEQNO] : (byte)(buffer [(int)TransCHKSUM.SEQNO] + 1) % 2);
			ackBuf [(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
			checksum.calcChecksum (ref ackBuf, (int)TransSize.ACKSIZE);
			link.send(ackBuf, (int)TransSize.ACKSIZE);
		}

		/// <summary>
		/// Send the specified buffer and size.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send(byte[] buf, int size)
		{
			//Send one frame at a time
			//Do not send more frames until ACK is received
			do
			{
				//Copy parameter buf to buffer, store from the fifth [4] bit onwards
				Array.Copy(buf, 0, buffer, (int)TransSize.ACKSIZE, size );

				buffer[(int)TransCHKSUM.SEQNO] = seqNo; //Add sequence number
				buffer[(int) TransCHKSUM.TYPE] = (byte) TransType.DATA; //Set type to DATA

				checksum.calcChecksum(ref buffer, size + (int) TransSize.ACKSIZE); //Add checksum to send buffer data

				if (GenerateNoiseEvery > 0 && (++noiseCounter % GenerateNoiseEvery) == 0)
				{
					//Generate noise
					buffer[(int) TransCHKSUM.CHKSUMHIGH] = 0;
					Console.WriteLine("Generated some noise, CHSUMHIGH is no longer legit");
				}

				link.send(buffer, size + (int) TransSize.ACKSIZE); //Send data

			} while (receiveAck() == false); //Kepp on going until sequence number changes

			old_seqNo = DEFAULT_SEQNO; //Increment old sequence number, to reset after sending

		}

		/// <summary>
		/// Receive the specified buffer.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public int receive (ref byte[] buf)
		{
			while (true)
			{
				int receivedBytes = link.receive(ref buffer); //Receive data from link layer

				if (checksum.checkChecksum(buffer, receivedBytes) == false)
				{
					//Checksum error, returning false ACK
					Console.WriteLine("Sending back false ack, checksum failed");
					Console.WriteLine("seqNo: {0}", buffer[(int)TransCHKSUM.SEQNO]);
					Console.WriteLine("old_seqNo: {0}", old_seqNo);

					sendAck(false); // Send false ack
					continue;
				}



				if (buffer[(int) TransCHKSUM.SEQNO] == old_seqNo)
				{
					Console.WriteLine("Wrong sequence number received, ignoring: {0}", buffer[(int) TransCHKSUM.SEQNO]);
					sendAck (false);
					continue;
				}

				//Correct data received
				sendAck(true);

				old_seqNo = buffer[(int) TransCHKSUM.SEQNO]; //Set old seqNo to the previous one

				Array.Copy(buffer, (int) TransSize.ACKSIZE, buf, 0, receivedBytes - 4); //Copy buffer to new buf

				return receivedBytes - 4; //return amount of bytes received
			}

		}
	}
}