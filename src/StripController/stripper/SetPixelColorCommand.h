#ifndef __SET_PIXEL_COLOR_COMMAND_H__
#define __SET_PIXEL_COLOR_COMMAND_H__

#include "Command.h"
#include "Strip.h"
#include "Logger.h"

class SetPixelColorCommand : public Command
{
private:
	Strip* _strip;
	Logger* _logger;

public:
	SetPixelColorCommand(Strip* strip, Logger* logger);

	int GetCommandId();
	void Handle(byte* command);
};

#endif