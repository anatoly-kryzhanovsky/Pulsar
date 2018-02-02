#ifndef __COMMAND_H__
#define __COMMAND_H__

#include <Arduino.h>

class Command
{
public:
	const static int ExecuteStripCommandId = 0;
	const static int SetBrighnessCommandId = 1;
	const static int SetPixelColorCommandId = 2;

public:
	virtual int GetCommandId() = 0;
	virtual void Handle(byte* command) = 0;
};

#endif