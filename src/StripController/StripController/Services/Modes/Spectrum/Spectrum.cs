using System;
using System.Collections.Generic;
using CSCore;
using CSCore.DSP;

namespace StripController.Services.Modes.Spectrum
{
    class Spectrum
    {
        private const double MinDbValue = -90;
        private const double MaxDbValue = 0;
        private const double DbScale = MaxDbValue - MinDbValue;
        private const int MaximumFrequency = 20000;
        private const int MinimumFrequency = 20;

        private readonly int _fftSize;
        private readonly int _maxFftIndex;
        private readonly SpectrumProvider _fftProvider;
        private readonly int _spectrumResolution;

        private int _maximumFrequencyIndex;
        private int _minimumFrequencyIndex;

        private int[] _spectrumIndexMax;
        private int[] _spectrumLogScaleIndexMax;

        private float[] _fftBuffer;

        public Spectrum(SpectrumProvider fftProvider, int channelCount)
        {
            _fftSize = (int)FftSize.Fft4096;
            _maxFftIndex = _fftSize / 2 - 1;
            
            _fftProvider = fftProvider;
            
            _spectrumResolution = channelCount;
            
            UpdateFrequencyMapping();
        }

        public bool FillFftData()
        {
            _fftBuffer = new float[_fftSize];
            return _fftProvider.GetFftData(_fftBuffer, this);
        }

        public IEnumerable<double> GetSpectrum()
        {
            var pixelColors = new double[_spectrumResolution];
            
            int height = 300;
            var spectrumPoints = CalculateSpectrumPoints(height, _fftBuffer);
                
            foreach (var p in spectrumPoints)
            {
                var barIndex = p.SpectrumPointIndex;
                pixelColors[barIndex] = p.Value / height;
            }

            return pixelColors;
        }

        protected virtual void UpdateFrequencyMapping()
        {
            _maximumFrequencyIndex = Math.Min(_fftProvider.GetFftBandIndex(MaximumFrequency) + 1, _maxFftIndex);
            _minimumFrequencyIndex = Math.Min(_fftProvider.GetFftBandIndex(MinimumFrequency), _maxFftIndex);

            int actualResolution = _spectrumResolution;

            int indexCount = _maximumFrequencyIndex - _minimumFrequencyIndex;
            double linearIndexBucketSize = Math.Round(indexCount / (double)actualResolution, 3);

            _spectrumIndexMax = _spectrumIndexMax.CheckBuffer(actualResolution, true);
            _spectrumLogScaleIndexMax = _spectrumLogScaleIndexMax.CheckBuffer(actualResolution, true);

            double maxLog = Math.Log(actualResolution, actualResolution);
            for (int i = 1; i < actualResolution; i++)
            {
                int logIndex =
                    (int)((maxLog - Math.Log((actualResolution + 1) - i, (actualResolution + 1))) * indexCount) +
                    _minimumFrequencyIndex;

                _spectrumIndexMax[i - 1] = _minimumFrequencyIndex + (int)(i * linearIndexBucketSize);
                _spectrumLogScaleIndexMax[i - 1] = logIndex;
            }

            if (actualResolution > 0)
            {
                _spectrumIndexMax[_spectrumIndexMax.Length - 1] =
                    _spectrumLogScaleIndexMax[_spectrumLogScaleIndexMax.Length - 1] = _maximumFrequencyIndex;
            }
        }

        protected virtual SpectrumPointData[] CalculateSpectrumPoints(double maxValue, float[] fftBuffer)
        {
            var dataPoints = new List<SpectrumPointData>();

            double value = 0;
            double lastValue = 0;
            double actualMaxValue = maxValue;
            int spectrumPointIndex = 0;

            for (int i = _minimumFrequencyIndex; i <= _maximumFrequencyIndex; i++)
            {
                var channelValue = (((20 * Math.Log10(fftBuffer[i])) - MinDbValue) / DbScale) * actualMaxValue;
                var recalc = true;

                value = Math.Max(0, Math.Max(channelValue, value));

                while (spectrumPointIndex <= _spectrumIndexMax.Length - 1 && i == _spectrumLogScaleIndexMax[spectrumPointIndex])
                {
                    if (!recalc)
                        value = lastValue;

                    if (value > maxValue)
                        value = maxValue;

                    if (spectrumPointIndex > 0)
                        value = (lastValue + value) / 2.0;

                    dataPoints.Add(new SpectrumPointData { SpectrumPointIndex = spectrumPointIndex, Value = value });

                    lastValue = value;
                    value = 0.0;
                    spectrumPointIndex++;
                    recalc = false;
                }
            }

            return dataPoints.ToArray();
        }
    }
}