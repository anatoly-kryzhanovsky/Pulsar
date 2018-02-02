#include "Strip.h"

Strip::Strip(int pixelCount, int pin, Logger* logger)
{
	_logger = logger;
	_strip = new Adafruit_NeoPixel(pixelCount, pin, NEO_BRG + NEO_KHZ800);
	_strip->begin();
	_colors.resize(pixelCount);
}

Strip::~Strip()
{
	delete _strip;
}

int Strip::GetPixelCount()
{
	return _strip->numPixels();
}

void Strip::Initialize()
{
	for (int i = 0; i < _colors.size(); i++)
		_colors[i] = Color(255, 255, 255);
		
	SetBrightness(255);
	Apply();	
}

void Strip::SetPixelColor(int pixel, int r, int g, int b)
{
	SetPixelColor(pixel, Color(r, g, b));
}

void Strip::SetPixelColor(int pixel, Color color)
{
	_colors[pixel] = color;
}

void Strip::SetBrightness(uint8_t brightness)
{
	_brightness = brightness;
}

void Strip::Apply()
{
	_strip->clear();
	_strip->setBrightness(_brightness);

	for (int i = 0; i < _colors.size(); i++)
	{
		Color color = _colors[i];
		_strip->setPixelColor(i, _strip->Color(color.getR(), color.getG(), color.getB()));
	}

	_strip->show();
}