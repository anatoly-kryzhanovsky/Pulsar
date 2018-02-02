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
	if ((_level & LevelTrace) != LevelTrace)
		return;

	sprintf(_buffer, "[TRACE] %s", msg);
	Serial.println(_buffer);
}

void Logger::Info(const char* msg)
{
	if ((_level & LevelInfo) != LevelInfo)
		return;

	sprintf(_buffer, "[INFO] %s", msg);
	Serial.println(_buffer);
}

void Logger::Error(const char* msg)
{
	if ((_level & LevelError) != LevelError)
		return;

	sprintf(_buffer, "[ERROR] %s", msg);
	Serial.println(_buffer);
}

void Logger::Warning(const char* msg)
{
	if ((_level & LevelWarning) != LevelWarning)
		return;

	sprintf(_buffer, "[WARNING] %s", msg);
	Serial.println(_buffer);
}