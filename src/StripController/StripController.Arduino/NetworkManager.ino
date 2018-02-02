#include <EthernetUdp.h>
#include <algorithm>
#include "NetworkManager.h"

NetworkManager::NetworkManager(char* mac, Logger* logger)
{
	_mac = mac;
	_logger = logger;
	_packetBuffer = new byte[4096];
}

NetworkManager::~NetworkManager()
{
	delete[] _packetBuffer;
}

bool NetworkManager::Initialize()
{
	_logger->Trace("NetworkManager initialization with DHCP begin");
	if (Ethernet.begin(_mac) == 0)
	{
		_logger->Warning("Failed to configure Ethernet using DHCP");
		return false;
	}

	_logger->Trace(IpToString(Ethernet.localIP()));
	_logger->Trace("NetworkManager initialization with DHCP end");
	return true;
}

void NetworkManager::Initialize(IPAddress ip, IPAddress mask, IPAddress gateway, IPAddress dns)
{
	_logger->Trace("NetworkManager initialization begin");
	_logger->Trace(IpToString(ip));
	Ethernet.begin(_mac, ip, dns, gateway, mask);
	_logger->Trace("NetworkManager initialization end");
}

void NetworkManager::AssignIP(IPAddress ip, IPAddress mask, IPAddress gateway, IPAddress dns)
{
	_logger->Trace("Manual assign IP begin");
	_logger->Trace(IpToString(ip));
	Ethernet.begin(_mac, ip, dns, gateway, mask);
	_logger->Trace("Manual assign IP end");
}

void NetworkManager::OpenUdpPort(int port)
{
	_logger->Trace("Open udp port begin");
	_udp.begin(port);
	_logger->Trace("Open udp port end");
}

char* NetworkManager::ReadPackage()
{
	char buffer[255];
	_logger->Trace("Read package begin");
	do {
		int packetSize = _udp.parsePacket();
		if (packetSize)
		{
			sprintf(buffer, "Receive package %d bytes", packetSize);
			_logger->Trace(buffer);

			_udp.read(_packetBuffer, UDP_TX_PACKET_MAX_SIZE);
			_logger->Trace("Read package end");

			return _packetBuffer;
		}
	} while (1);
}

char* NetworkManager::IpToString(IPAddress address)
{
	char buffer[16];
	sprintf(buffer, "%d.%d.%d.%d", address[0], address[1], address[2], address[3]);

	return buffer;
}