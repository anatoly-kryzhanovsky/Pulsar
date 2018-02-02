#include "Color.h"

Color::Color()
	:_r(0), _g(0), _b(0)
{
}

Color::Color(byte r, byte g, byte b)
	:_r(r), _g(g), _b(b)
{
}

byte Color::getR()
{
	return _r;
}

byte Color::getG()
{
	return _g;
}

byte Color::getB()
{
	return _b;
}