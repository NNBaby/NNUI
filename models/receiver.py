import socket

address = ('127.0.0.1', 3939)  
s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  
s.bind(address)

print ("Listen...")
while True:
    data, addr = s.recvfrom(2048)
    print (data.decode("UTF-8"))