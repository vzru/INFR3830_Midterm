#include "Server.h"

Server::Server()
{
	WSADATA data;
	WORD version = MAKEWORD(2, 2);

	//startup
	int wsOk = WSAStartup(version, &data);
	if (wsOk != 0) {
		std::cout << "cant start winsock" << wsOk;
		return;
	}

	//TCP socket connection
	//socket setup
	serverTCP.sin_addr.S_un.S_addr = ADDR_ANY;
	serverTCP.sin_family = AF_INET;
	serverTCP.sin_port = htons(55555);
	TCP = socket(serverTCP.sin_family, SOCK_STREAM, IPPROTO_TCP);
	if (TCP == INVALID_SOCKET)
	{
		std::cout << "Can't create a socket! " << WSAGetLastError() << std::endl;
		WSACleanup();
		return;
	}

	if (bind(TCP, (sockaddr*)&serverTCP, sizeof(serverTCP)) == SOCKET_ERROR) {
		std::cout << "Can't bind socket! " << WSAGetLastError() << std::endl;					//bind client info to client
		closesocket(TCP);
		WSACleanup();
		return;
	}

	//UDP socket connection
	//socket setup
	serverUDP.sin_addr.S_un.S_addr = ADDR_ANY;
	serverUDP.sin_family = AF_INET;
	serverUDP.sin_port = htons(60000);
	UDP = socket(serverUDP.sin_family, SOCK_DGRAM, IPPROTO_UDP);
	if (UDP == INVALID_SOCKET)
	{
		std::cout << "Can't create a socket! " << WSAGetLastError() << std::endl;
		WSACleanup();
		return;
	}

	if (bind(UDP, (sockaddr*)&serverUDP, sizeof(serverUDP)) == SOCKET_ERROR) {
		std::cout << "Can't bind socket! " << WSAGetLastError() << std::endl;					//bind client info to client
		closesocket(UDP);
		WSACleanup();
		return;
	}

	listen(TCP, SOMAXCONN);

	//zero the master list, then add in the listener socket
	FD_ZERO(&master);
	FD_SET(TCP, &master);
	FD_SET(UDP, &master);
}

Server::~Server()
{
}


void Server::acceptTCPConnection()
{
	std::cout << "TCP Connection" << std::endl;

	sockaddr_in tmpAddress;
	int tmpLen = sizeof(tmpAddress);

	// Accept a new connection
	SOCKET client = accept(TCP, (struct sockaddr*) & tmpAddress, &tmpLen);
	FD_SET(client, &master);
}

void Server::startUpdates()
{
	while (listening) {
		fd_set copy = master;

		timeval nope;
		nope.tv_sec = 0;
		nope.tv_usec = 0;
		int socketCount = select(0, &copy, nullptr, nullptr, &nope);

		//handle all active sockets
		for (int i = 0; i < socketCount; i++)
		{
			// std::cout << "Packet Recieved:";

			SOCKET sock = copy.fd_array[i];

			//Socket Listener
			if (sock == UDP) {
				char* buf = new char[DEFAULT_PACKET_SIZE];
				sockaddr_in fromAddr;
				int fromLen = sizeof(fromAddr);

				//recieve packet from socket
				int sError = recvfrom(UDP, buf, DEFAULT_PACKET_SIZE, 0, (struct sockaddr*) & fromAddr, &fromLen);

				if (sError == SOCKET_ERROR) {
					std::cout << "UDP Recieve Error: " << WSAGetLastError() << std::endl;
				}
				else {
					//packetUDP(buf, fromAddr, fromLen);
				}

				delete[] buf;
			}
			else if (sock == TCP)
			{
					acceptTCPConnection();
			}
			//TCP Input Sockets
			else {
				char* buf = new char[DEFAULT_PACKET_SIZE];

				int byteReceived = recv(sock, buf, DEFAULT_PACKET_SIZE, 0);
				//handles disconnection packets
				if (byteReceived <= 0)
				{
					std::cout << "Disconnection" << std::endl;

					// Drop the client
					closesocket(sock);
					FD_CLR(sock, &master);
				}
				else if (byteReceived == SOCKET_ERROR) {
					std::cout << "Recieve Error: " << WSAGetLastError() << std::endl;
				}
				else {
					//process packet data
					int length;
					memcpy(&length, buf, sizeof(length));

					std::cout << "LENGTH: " << length << std::endl;
					//packetTCP(buf);
				}

				delete[] buf;
			}
		}
	}
}
