#ifndef __SET_BRIGHTNESS_COMMAND_H__
#define __SET_BRIGHTNESS_COMMAND_H__

#include "Command.h"
#include "Strip.h"
#include "Logger.h"

class SetBrighnessCommand : public Command
{
private:
	Strip* _strip;
	Logger* _logger;

public:
	SetBrighnessCommand(Strip* strip, Logger* logger);

	int GetCommandId();
	void Handle(byte* command);
};

#endif