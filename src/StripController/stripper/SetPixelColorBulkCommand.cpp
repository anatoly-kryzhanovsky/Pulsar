#include "SetPixelColorBulkCommand.h"

SetPixelColorBulkCommand::SetPixelColorBulkCommand(Strip* strip, Logger* logger)
	:_strip(strip), _logger(logger)
{
}

int SetPixelColorBulkCommand::GetCommandId()
{
	return Command::SetPixelColorBulkCommandId;
}

void SetPixelColorBulkCommand::Handle(byte* command)
{
	_logger->Trace("Handle SetPixelColorBulkCommand begin");

	int offset = 1;

	byte start = command[offset++];
	byte end = command[offset++];
	byte r = command[offset++];
	byte g = command[offset++];
	byte b = command[offset++];

	char buffer[255];
	sprintf(buffer, "Arguments: {\"start\": %d, \"end\": %d, \"r\": %d, \"g\": %d, \"b\": %d}", start, end, r, g, b);
	_logger->Trace(buffer);

	for(int i = start; i < end; i++)
		_strip->SetPixelColor(i, r, g, b);

	_logger->Trace("Handle SetPixelColorBulkCommand end");
}