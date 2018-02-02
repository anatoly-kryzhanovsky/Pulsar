#ifndef __SET_STRIP_STATE_COMMAND_H__
#define __SET_STRIP_STATE_COMMAND_H__

#include "Command.h"
#include "Strip.h"
#include "Logger.h"

class SetStripStateCommand : public Command
{
private:
	Strip* _strip;
	Logger* _logger;

public:
	SetStripStateCommand(Strip* strip, Logger* logger);

	int GetCommandId();
	void Handle(byte* command);
};

#endif