import sys
import socket
from TCPLib import *

HOST = "10.0.0.1"
PORT = 9000
BUFSIZE = 1000


def main(argv):
    serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    serversocket.bind((HOST, PORT))

    serversocket.listen(5)


    while True:
        # Accepting Connection
        clientsocket, addr = serversocket.accept()

        print("got a connection from {}".format(addr))

        # Getting file request
        msg = readTextTCP(clientsocket)

        print("message received: {}".format(msg))

        # If close, close server, used for debugging and quick resetting the server
        if msg == "close":
            serversocket.close()
            sys.exit()

        # Getting filesize of the file, will return 0, if file not found
        size = check_File_Exists(msg)

        if size > 0:
            returnmsg = str(size)  # Making the size a string to be sent
            writeTextTCP(returnmsg, clientsocket)  # Sending filesize as a string

            file = open(msg, 'rb') # Opens file

            filedata = file.read(BUFSIZE)  # Reads BUFSIZE bytes in from the file

            while filedata:
                clientsocket.send(filedata)  # Send filedata
                filedata = file.read(BUFSIZE)  # Read more bytes.


        #writeTextTCP(msg, clientsocket)
        clientsocket.close()

        


def sendFile(fileName,  fileSize,  conn):
    pass
    # TO DO Your Code
    
if __name__ == "__main__":
    main(sys.argv[1:])
