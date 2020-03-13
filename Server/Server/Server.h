#pragma once

#include <WinSock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <string>
#include <iostream>
#include <thread>
#include <queue>
#include "packet.h"

#pragma comment(lib, "Ws2_32.lib")

#define DEFAULT_PORT 8888
#define DEFAULT_PACKET_SIZE 512
#define PACKET_LENGTH 0
#define PACKET_TYPE 4
#define PACKET_SENDER 8
#define INITIAL_OFFSET 12

struct UserProfile {
	int index;
	std::string Username;
	SOCKET tcpSocket;
	sockaddr_in tcpAddress;
	int tcpLength;
	sockaddr_in udpAddress;
	std::string clientIP;
	int udpLength;

	//checks for disconnection
	bool active = false;
	bool activeUDP = false;
};

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

	void startListening();

	void startUpdates();

	void packetTCP(char* packet);

	void packetUDP(char* packet, sockaddr_in fromAddr, int fromLen);

	void acceptNewClient(int sender, sockaddr_in address, int length);

	void PackAuxilaryData(char* buffer, int length, int sender, int type);

private:
	fd_set master;
	bool init = false;
	bool listening;
	int clientCount;
	std::vector<UserProfile> ConnectedUsers;


	template<class T>
	void PackData(char* buffer, int* loc, T data);
};