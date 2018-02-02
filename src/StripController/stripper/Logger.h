#ifndef __LOGGER_H__
#define __LOGGER_H__

class Logger
{
public:
	static const int LevelNone		= 0;
	static const int LevelTrace		= 1;
	static const int LevelInfo		= 2;	
	static const int LevelWarning	= 4;
	static const int LevelError		= 8;

private:
	char* _buffer;
	int _level;

public:
	Logger(int boundRate);
	~Logger();

	void SetLogLevel(int level);

	void Trace(const char* msg);
	void Info(const char* msg);	
	void Warning(const char* msg);
	void Error(const char* msg);
};

#endif