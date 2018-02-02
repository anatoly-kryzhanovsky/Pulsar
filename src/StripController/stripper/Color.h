#ifndef __COLOR_H__
#define __COLOR_H__

#include <Arduino.h>

class Color
{
private:
	byte _r;
	byte _g;
	byte _b;

public:
	Color();
	Color(byte r, byte g, byte b);

	byte getR();
	byte getG();
	byte getB();
};

#endif