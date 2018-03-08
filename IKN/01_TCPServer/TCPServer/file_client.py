import sys
import socket
from TCPLib import *

PORT = 9000
BUFSIZE = 1000


def main(argv):
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

	try:
		clientsocket.connect((argv[0], PORT))
	except:
		print("could not connect to specified server")
		
		
    if argv:
        msg = "".join(argv[1])
    else:
        sys.exit()

    writeTextTCP(msg, clientsocket)

    filesize = getFileSizeTCP(clientsocket)
    dataread = 0

    file = open(msg, 'wb')

    while dataread < filesize:
        filedata = clientsocket.recv(BUFSIZE)
        file.write(filedata)
        dataread = dataread+len(filedata)

    clientsocket.close()

    print(msg)

def receiveFile(fileName, conn):
    pass


if __name__ == "__main__":
    main(sys.argv[1:])
