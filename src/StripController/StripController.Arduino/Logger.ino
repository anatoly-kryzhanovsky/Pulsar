#include <stdio.h>
#include <stdlib.h>
#include <Arduino.h>

#include "Logger.h"

Logger::Logger(int boundRate)
{
	Serial.begin(boundRate);
	_buffer = new char[512];
}

Logger::~Logger()
{
	delete[] _buffer;
}

void Logger::SetLogLevel(int level)
{
	_level = level;
}

void Logger::Trace(const char* msg)
{
	if (_level < LevelTrace)
		return;

	sprintf(_buffer, "[TRACE] %s", msg);
	Serial.println(msg);
}

void Logger::Error(const char* msg)
{
	if (_level < LevelError)
		return;

	sprintf(_buffer, "[ERROR] %s", msg);
	Serial.println(msg);
}

void Logger::Warning(const char* msg)
{
	if (_level < LevelWarning)
		return;

	sprintf(_buffer, "[WARNING] %s", msg);
	Serial.println(msg);
}