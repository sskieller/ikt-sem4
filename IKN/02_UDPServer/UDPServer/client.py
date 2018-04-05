import sys
import socket


PORT = 9000
BUFSIZE = 1000


def main(argv):
    #Create socket
    clientsocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    #Get server addr
    server_addr = (argv[0], PORT)

    if argv:
        msg = "".join(argv[1])
        print(msg)
    else:
        sys.exit()

    #Send message to server
    clientsocket.sendto(msg.encode('UTF-8', 'strict'), server_addr)
    #Receive data from server
    msg, addr = clientsocket.recvfrom(BUFSIZE)
    
    #Close socket
    clientsocket.close()
    #Print message
    print(msg)


if __name__ == "__main__":
    main(sys.argv[1:])
