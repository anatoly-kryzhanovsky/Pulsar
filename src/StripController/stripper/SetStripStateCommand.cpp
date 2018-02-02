#include "SetStripStateCommand.h"

SetStripStateCommand::SetStripStateCommand(Strip* strip, Logger* logger)
	:_strip(strip), _logger(logger)
{
}

int SetStripStateCommand::GetCommandId()
{
	return Command::SetStripStateCommandId;
}

void SetStripStateCommand::Handle(byte* command)
{
	_logger->Trace("Handle SetStripStateCommand begin");

	int offset = 1;
	
	byte brightness = command[offset++];
	byte pixelsCount = command[offset++];
	
	for (int i = 0; i < pixelsCount; i++)
	{
		byte r = command[offset++];
		byte g = command[offset++];
		byte b = command[offset++];

		_strip->SetPixelColor(i, r, g, b);
	}	

	_strip->SetBrightness(brightness);
	_strip->Apply();

	_logger->Trace("Handle SetStripStateCommand end");
}