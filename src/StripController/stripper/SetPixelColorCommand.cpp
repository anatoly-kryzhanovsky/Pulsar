#include "SetPixelColorCommand.h"

SetPixelColorCommand::SetPixelColorCommand(Strip* strip, Logger* logger)
	:_strip(strip), _logger(logger)
{
}

int SetPixelColorCommand::GetCommandId()
{
	return Command::SetPixelColorCommandId;
}

void SetPixelColorCommand::Handle(byte* command)
{
	_logger->Trace("Handle SetPixelColorCommand begin");

	int offset = 1;

	byte pixel = command[offset++];
	byte r = command[offset++];
	byte g = command[offset++];
	byte b = command[offset++];

	char buffer[255];
	sprintf(buffer, "Arguments: {\"pixel\": %d, \"r\": %d, \"g\": %d, \"b\": %d}", pixel, r, g, b);
	_logger->Trace(buffer);

	_strip->SetPixelColor(pixel, r, g, b);

	_logger->Trace("Handle SetPixelColorCommand end");
}