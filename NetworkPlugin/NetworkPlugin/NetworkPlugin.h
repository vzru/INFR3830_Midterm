#pragma once

#include <iostream>
#include <thread>
#include <WinSock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#pragma comment(lib, "Ws2_32.lib")

#ifdef PLUGIN_EXPORTS
#define PLUGIN_H __declspec(dllexport)
#else
#define PLUGIN_H
#endif

#define DEFAULT_PORT "8888"
#define DEFAULT_PACKET_SIZE 512

class NetworkPlugin
{
public:
	NetworkPlugin();
	~NetworkPlugin();

	SOCKET TCP;
	struct addrinfo* ptrTCP = NULL, hintsTCP;
	SOCKET UDP;
	struct addrinfo* ptrUDP = NULL, hintsUDP;

	bool connectServer(std::string ip);
	bool sendPackets(char* ptr, int length, bool useTCP);

	void update();

	void showConsole(bool open);
private:
	std::string DefaultServerIP = "127.0.0.1";
	bool init = false;
	bool socketInit = false;
	bool consoleOpen = false;
	bool listening = true;
};


extern "C" {
	void (*receivePacket)(char* buffer, int length, bool useTCP);

	PLUGIN_H NetworkPlugin* CreateClient();
	PLUGIN_H void DeleteClient(NetworkPlugin* client);
	PLUGIN_H void ShowConsole(NetworkPlugin* client, bool show);
	PLUGIN_H bool Connect(NetworkPlugin* client, char* ip);
	PLUGIN_H bool SendPackets(NetworkPlugin* client, char* buffer, int length, bool useTCP);
	PLUGIN_H void Update(NetworkPlugin* client);
	PLUGIN_H void SetupPacketReception(void(*action)(char* buffer, int length, bool useTCP));
}