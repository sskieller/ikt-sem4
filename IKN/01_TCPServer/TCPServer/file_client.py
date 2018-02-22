import sys
import socket
from lib import Lib

PORT = 9000
BUFSIZE = 1000


def main(argv):
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    clientsocket.connect(("10.0.0.1", PORT))

    clientsocket.send("hello world".encode('ascii'))

    msg = clientsocket.recv(1000)

    clientsocket.close()

    print(msg.decode('ascii'))

def receiveFile(fileName, conn):
    pass


if __name__ == "__main__":
    main(sys.argv[1:])
