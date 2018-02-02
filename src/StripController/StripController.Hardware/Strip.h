#ifndef __STRIP_H__
#define __STRIP_H__

#include <Adafruit_NeoPixel.h>
#include <StandardCplusplus.h>
#include <vector>

#include "Logger.h"

class Strip
{
private:
	std::vector<int> _colors;
	Adafruit_NeoPixel* _strip;
	uint8_t _brightness;
	Logger* _logger;

public:
	Strip(int pixelCount, int pin, Logger* logger);
	~Strip();

	void Initialize();
	void SetPixelColor(int pixel, int color);
	void SetPixelColor(int pixel, int r, int g, int b);
	void SetBrightness(uint8_t brightness);
	void Apply();

	int GetPixelCount();
};

#endif