#include <StandardCplusplus.h>
#include <system_configuration.h>
#include <unwind-cxx.h>
#include <utility.h>
#include <vector>
#include <Ethernet.h>

#include "Command.h"
#include "Logger.h"
#include "NetworkManager.h"
#include "Strip.h"

#include "ExecuteStripCommand.h"
#include "SetBrighnessCommand.h"
#include "SetPixelColorCommand.h"

const int DataPin = 6;											// командный пин ленты
const int PixelCount = 50;										// количество адресуемых сегментов в ленте
const byte mac[] = { 0x00, 0xAA, 0xBB, 0xCC, 0xDE, 0x02 };		// mac 
const int IncomingPort = 50000;                                 // порт для входящих подключений

const IPAddress ip(192, 168, 0, 127);                           // ip если dhcp не доступен
const IPAddress myDns(192, 168, 0, 1);                          // dns если dhcp не доступен
const IPAddress gateway(192, 168, 0, 1);                        // шлюз если dhcp не доступен
const IPAddress subnet(255, 255, 255, 0);                       // маска подсети если dhcp не доступен

Logger* logger;
Strip* strip;
NetworkManager* network;
std::vector<Command*> handlers;

void setup() {
	logger = new Logger(9600);
	network = new NetworkManager(mac, logger);
	strip = new Strip(PixelCount, DataPin, logger);

	logger->SetLogLevel(Logger::LevelNone);

	if (!network->Initialize())
		network->AssignIP(ip, subnet, gateway, myDns);

	network->OpenUdpPort(IncomingPort);
	strip->Initialize();
	strip->SetMode(1);

	delay(5000);

	logger->Trace("Initialization done");

	handlers.push_back(new ExecuteStripCommand(strip, logger));
	handlers.push_back(new SetBrighnessCommand(strip, logger));
	handlers.push_back(new SetPixelColorCommand(strip, logger));
}

void loop() {
	char* package = network->ReadPackage();
	for (int i = 0; i < handlers.size(); i++)
	{
		if (handlers[i]->GetCommandId() == package[0])
			handlers[i]->Handle(package);
	}
}