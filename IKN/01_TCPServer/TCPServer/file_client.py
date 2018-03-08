import sys
import socket
from TCPLib import *

PORT = 9000
BUFSIZE = 1000


def main(argv):
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    if len(argv)==2:
        server = argv[0]
        msg = argv[1]
    else:
        print("Format needs to be <server IP> <Path/Filename>")
        sys.exit()

    clientsocket.connect((server, PORT))

    writeTextTCP(msg, clientsocket)

    filesize = getFileSizeTCP(clientsocket)
    dataread = 0
    percent = 0

    if filesize <= 0:
        print("File not found on server")
        sys.exit()

    file = open(extractFilename(msg), 'wb')

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
