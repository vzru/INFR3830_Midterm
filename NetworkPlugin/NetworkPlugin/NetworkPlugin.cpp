#include "NetworkPlugin.h"

NetworkPlugin::NetworkPlugin()
{
	WSADATA data;
	WORD version = MAKEWORD(2, 2);

	//startup
	int wsOk = WSAStartup(version, &data);
	if (wsOk != 0) {
		std::cout << "Failed to Start Winsock: " << wsOk << std::endl;
		return;
	}

	memset(&hintsTCP, 0, sizeof(hintsTCP));
	hintsTCP.ai_family = AF_INET;
	hintsTCP.ai_socktype = SOCK_STREAM;
	hintsTCP.ai_protocol = IPPROTO_TCP;

	memset(&hintsUDP, 0, sizeof(hintsUDP));
	hintsUDP.ai_family = AF_INET;
	hintsUDP.ai_socktype = SOCK_DGRAM;
	hintsUDP.ai_protocol = IPPROTO_UDP;
}

NetworkPlugin::~NetworkPlugin()
{
	listening = false;
	closesocket(TCP);
	closesocket(UDP);
	WSACleanup();
	showConsole(false);
}

bool NetworkPlugin::connectServer(std::string ip)
{
	if (socketInit) {
		return true;
	}

	if (ip != "")
	{
		DefaultServerIP = ip;
	}

	std::cout << "ConnectingTCP..." << std::endl;
	if (getaddrinfo(DefaultServerIP.c_str(), DEFAULT_PORT, &hintsTCP, &ptrTCP) != 0)
	{
		std::cout << "Getaddrinfo TCP Failed! " << WSAGetLastError() << std::endl;
		WSACleanup();
		return false;
	}

	TCP = socket(ptrTCP->ai_family, ptrTCP->ai_socktype, ptrTCP->ai_protocol);
	if (TCP == INVALID_SOCKET)
	{
		std::cout << "Can't create TCP socket, Err #" << WSAGetLastError() << std::endl;
		WSACleanup();
		return false;
	}

	if (connect(TCP, ptrTCP->ai_addr, (int)ptrTCP->ai_addrlen) == SOCKET_ERROR)
	{
		//std::cout << "TCP Socket failed to connect to server, Err #" << WSAGetLastError() << std::endl;
		closesocket(TCP);
		freeaddrinfo(ptrTCP);
		WSACleanup();

		std::cout << "Connection Failed!" << std::endl;

		return false;
	}
	else
	{
		socketInit = true;
	}
	std::cout << "TCP Connected!" << std::endl;

	std::cout << "ConnectingUDP..." << std::endl;
	if (getaddrinfo(DefaultServerIP.c_str(), "8889", &hintsUDP, &ptrUDP) != 0)
	{
		WSACleanup();
		return false;
	}

	UDP = socket(ptrUDP->ai_family, ptrUDP->ai_socktype, ptrUDP->ai_protocol);
	if (UDP == INVALID_SOCKET)
	{
		std::cout << "Can't create UDP socket, Err #" << WSAGetLastError() << std::endl;
		WSACleanup();
		return false;
	}
	std::cout << "Connected UDP!" << std::endl;

	//ping and determine client index
	init = true;
	return true;
}

bool NetworkPlugin::sendPackets(char* ptr, int length, bool useTCP)
{
	if (init)
	{
		if (!useTCP) {
			//PrintPackInfo(1, ptr, length);
			if (sendto(UDP, ptr, DEFAULT_PACKET_SIZE, 0, ptrUDP->ai_addr, (int)ptrUDP->ai_addrlen) == SOCKET_ERROR) {
				std::cout << "Send Error: " << WSAGetLastError() << std::endl;
				return false;
			}
		}
		//tcp send
		else {
			if (send(TCP, ptr, DEFAULT_PACKET_SIZE, 0) == SOCKET_ERROR)
			{
				std::cout << "Send Error: " << WSAGetLastError() << std::endl;
				return false;
			}
		}
		return true;
	}
	return false;
}

void NetworkPlugin::update()
{
	std::thread tcpUpdate = std::thread([&]()
		{
			char* buff = new char[DEFAULT_PACKET_SIZE];

			while (listening) {
				//receive packets
				int sError = recv(TCP, buff, DEFAULT_PACKET_SIZE, 0);
				if (sError != SOCKET_ERROR) {
					int pLength = 0;
					memcpy(&pLength, &buff[0], sizeof(pLength));
					receivePacket(buff, pLength, true);
				}
			}

			delete[] buff;
		});
	tcpUpdate.detach();


	std::thread udpUpdate = std::thread([&]()
		{
			char* buff = new char[DEFAULT_PACKET_SIZE];

			while (listening) {
				//receive messages
				int sError = recv(UDP, buff, DEFAULT_PACKET_SIZE, 0);

				if (sError != SOCKET_ERROR) {
					int pLength = 0;
					memcpy(&pLength, &buff[0], sizeof(pLength));
					receivePacket(buff, pLength, false);
				}
			}

			delete[] buff;
		});
	udpUpdate.detach();
}

void NetworkPlugin::showConsole(bool open)
{
	if (open && !consoleOpen)
	{
		FILE* pConsole;
		AllocConsole();
		freopen_s(&pConsole, "CONOUT$", "wb", stdout);
		consoleOpen = true;
	}
	else if (!open && consoleOpen)
	{
		FreeConsole();
		consoleOpen = false;
	}
}

PLUGIN_H NetworkPlugin* CreateClient()
{
	return new NetworkPlugin();
}

PLUGIN_H void DeleteClient(NetworkPlugin* client)
{
	delete client;
}

PLUGIN_H void ShowConsole(NetworkPlugin* client, bool show)
{
	client->showConsole(show);
}

PLUGIN_H bool Connect(NetworkPlugin* client, char* ip)
{
	return client->connectServer(ip);
}

PLUGIN_H bool SendPackets(NetworkPlugin* client, char* buffer, int length, bool useTCP)
{
	return client->sendPackets(buffer, length, useTCP);
}

PLUGIN_H void Update(NetworkPlugin* client)
{
	client->update();
}

PLUGIN_H void SetupPacketReception(void(*action)(char* buffer, int length, bool useTCP))
{
	receivePacket = action;
}
