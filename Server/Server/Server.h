#pragma once

#include <WinSock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <string>
#include <iostream>
#include <thread>
#include <queue>

#pragma comment(lib, "Ws2_32.lib")

#define DEFAULT_PORT "8888"
#define DEFAULT_PACKET_SIZE 512

class Server
{
public:
	Server();
	~Server();

	//UDP Socket
	SOCKET UDP;
	sockaddr_in serverUDP;

	SOCKET TCP;
	sockaddr_in serverTCP;

	void acceptTCPConnection();

	void startUpdates();

private:
	fd_set master;
	bool listening;
};