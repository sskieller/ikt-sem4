import sys
import socket


PORT = 9000
BUFSIZE = 1000


def main(argv):
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    
    server_addr = (argv[0], PORT)

    if argv:
        msg = "".join(argv[1])
        print(msg)
    else:
        sys.exit()


    clientsocket.sendto(msg.encode('UTF-8', 'strict'), server_addr)

    msg, addr = clientsocket.recvfrom(BUFSIZE)
    

    clientsocket.close()

    print(msg)

def receiveFile(fileName, conn):
    pass


if __name__ == "__main__":
    main(sys.argv[1:])
