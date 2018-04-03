import sys
import socket

HOST = "10.0.0.1"
PORT = 9000
BUFSIZE = 1000


def main(argv):
    #Create socket
    serversocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    serversocket.bind((HOST, PORT))


    #Keep server on
    while True:
        msg, addr = serversocket.recvfrom(BUFSIZE)

        print("message received: {}".format(msg))

        # If close, close server, used for debugging and quick resetting the server
        if msg == "close":
            serversocket.close()
            sys.exit()

        elif msg == 'L' or msg == 'l':
            file = open("/proc/uptime", 'r')
            serversocket.sendto(file.read().encode('UTF-8', 'strict'), addr)

        elif msg == 'U' or msg == 'u':
            file = open("/proc/loadavg", 'r')
            serversocket.sendto(file.read().encode('UTF-8', 'strict'), addr)
        else:
            file = 'Error'
            serversocket.sendto(file.encode('UTF-8', 'strict'), addr)


if __name__ == "__main__":
    main(sys.argv[1:])
