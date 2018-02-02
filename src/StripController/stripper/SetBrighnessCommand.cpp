#include "SetBrighnessCommand.h"

SetBrighnessCommand::SetBrighnessCommand(Strip* strip, Logger* logger)
	:_strip(strip), _logger(logger)
{
}

int SetBrighnessCommand::GetCommandId()
{
	return Command::SetBrighnessCommandId;
}

void SetBrighnessCommand::Handle(byte* command)
{
	_logger->Trace("Handle SetBrighnessCommand begin");

	int offset = 1;
	byte value = command[offset];

	char buffer[255];
	sprintf(buffer, "Arguments: {\"value\": %d}", value);
	_logger->Trace(buffer);

	_strip->SetBrightness(value);

	_logger->Trace("Handle SetBrighnessCommand end");
}