import sys
import socket
from TCPLib import *

PORT = 9000
BUFSIZE = 1000


def main(argv):
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    clientsocket.connect(("10.0.0.1", PORT))

    msg = "hello world"

    writeTextTCP(msg, clientsocket)

    msg = readTextTCP(clientsocket)

    clientsocket.close()

    print(msg)

def receiveFile(fileName, conn):
    pass


if __name__ == "__main__":
    main(sys.argv[1:])
