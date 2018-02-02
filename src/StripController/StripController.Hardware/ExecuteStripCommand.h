#ifndef __EXECUTE_STRIP_COMMAND_H__
#define __EXECUTE_STRIP_COMMAND_H__

#include "Command.h"
#include "Strip.h"
#include "Logger.h"

class ExecuteStripCommand : public Command
{
private:
	Strip* _strip;
	Logger* _logger;

public:
	ExecuteStripCommand(Strip* strip, Logger* logger);

	int GetCommandId();
	void Handle(byte* command);
};

#endif