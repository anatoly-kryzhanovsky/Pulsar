#ifndef __SET_PIXEL_COLOR_BULK_COMMAND_H__
#define __SET_PIXEL_COLOR_BULK_COMMAND_H__

#include "Command.h"
#include "Strip.h"
#include "Logger.h"

class SetPixelColorBulkCommand : public Command
{
private:
	Strip* _strip;
	Logger* _logger;

public:
	SetPixelColorBulkCommand(Strip* strip, Logger* logger);

	int GetCommandId();
	void Handle(byte* command);
};

#endif