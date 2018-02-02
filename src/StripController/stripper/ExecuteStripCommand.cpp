#include "ExecuteStripCommand.h"

ExecuteStripCommand::ExecuteStripCommand(Strip* strip, Logger* logger)
	:_strip(strip), _logger(logger)
{
}

int ExecuteStripCommand::GetCommandId()
{
	return Command::ExecuteStripCommandId;
}

void ExecuteStripCommand::Handle(byte* command)
{
	_logger->Trace("Handle ExecuteStripCommand begin");	
	_strip->Apply();
	_logger->Trace("Handle ExecuteStripCommand end");
}