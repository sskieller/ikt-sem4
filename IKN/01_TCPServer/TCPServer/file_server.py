import sys
import socket
from TCPLib import *

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

        msg = readTextTCP(clientsocket)

        print("message received: ", msg)

        if msg == "close":
            serversocket.close()
            sys.exit()
        size = check_File_Exists(extractFilename(msg))

        if size > 0:
            msg = str(size)
        else:
            msg = "Hello client!"

        writeTextTCP(msg, clientsocket)
        clientsocket.close()

        


def sendFile(fileName,  fileSize,  conn):
    pass
    # TO DO Your Code
    
if __name__ == "__main__":
    main(sys.argv[1:])