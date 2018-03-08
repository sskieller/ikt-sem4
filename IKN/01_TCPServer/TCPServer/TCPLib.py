import sys
import string
import os

def extractFilename(fileName):
    return fileName[fileName.rfind('/', 0,  len(fileName))+1:]

def check_File_Exists(fileName):
    try:
        size = os.stat(fileName).st_size
    except:
        size = 0
        #sys.exc_clear()

    return size

def readTextTCP(client):
    text = ""
    ch = client.recv(1)

    while ch.decode('UTF-8', 'strict') != "\0":
        text += ch.decode('UTF-8', 'strict')
        ch = client.recv(1)

    return text

def writeTextTCP(text,  client):
    client.send((text+"\0").encode('UTF-8', 'strict'))

def getFileSizeTCP(client):
    filesize = 0
    try:
        filesize = int(readTextTCP(client))
    except:
        filesize = -1
        sys.exc_clear()

    return filesize
