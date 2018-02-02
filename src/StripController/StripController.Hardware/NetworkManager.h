#ifndef __NETWORK_MANAGER_H__
#define __NETWORK_MANAGER_H__

#include <Arduino.h>
#include <SPI.h> 
#include <Ethernet.h>
#include <EthernetUdp.h>

#include "Logger.h"

class NetworkManager
{
private:
	byte* _packetBuffer;
	char* _mac;
	Logger* _logger;
	EthernetUDP _udp;

public:
	NetworkManager(char* mac, Logger* logger);
	~NetworkManager();

	bool Initialize();
	void Initialize(IPAddress ip, IPAddress mask, IPAddress gateway, IPAddress dns);
	void AssignIP(IPAddress ip, IPAddress mask, IPAddress gateway, IPAddress dns);
	void OpenUdpPort(int port);

	char* ReadPackage();

private:
	char* IpToString(IPAddress address);
};

#endif