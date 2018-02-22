import sys
import socket
from lib import Lib

HOST = ''
PORT = 9000
BUFSIZE = 1000

def main(argv):
    serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    serversocket.bind(("10.0.0.1", 9000))

    serversocket.listen(5)


    while True:
        clientsocket, addr = serversocket.accept()

        print("got a connection from %s", addr)

        msg = clientsocket.recv(1000)

        print("message received: %s", msg.decode('ascii'))

        msg = "Hello client!"
        clientsocket.send(msg.encode('ascii'))
        clientsocket.close()


def sendFile(fileName,  fileSize,  conn):
    pass
    # TO DO Your Code
    
if __name__ == "__main__":
    main(sys.argv[1:])