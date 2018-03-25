using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using StripController.Infrastructure.StripWrapper;
using StripController.PresentationEntities;
using StripController.Services.Modes.Spectrum;

namespace StripController.Services.Modes
{
    class AudioCaptureMode : IAudioCaptureMode
    {
        private WasapiCapture _soundIn;
        private IWaveSource _source;
        private Timer _timerThread;
        private double _totalVolume;

        private Spectrum.Spectrum _spectrum;
        private double _sensivity;
        private IEnumerable<GradientPointPe> _gradient;

        public event SpectrumUpdatedEventHandler SpectrumUpdated;

        public IStripper Stripper { get; }

        public AudioCaptureMode(IStripper stripper)
        {
            Stripper = stripper;
            Initialize();
        }

        public void SetGradient(IEnumerable<GradientPointPe> gradient)
        {
            _gradient = gradient
                .OrderBy(x => x.Value)
                .ToArray();
        }

        public void SetSensivite(double value)
        {
            _sensivity = value;
        }

        public void Start()
        {
            _totalVolume = Stripper.PixelCount * 0.25;
            _soundIn.Start();

            _timerThread = new Timer(Tick);
            _timerThread.Change(0, 40);
        }

        public void Stop()
        {
            _timerThread?.Dispose();
            _soundIn.Stop();
        }

        private void Initialize()
        {
            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Initialize();

            var soundInSource = new SoundInSource(_soundIn);
            var source = soundInSource.ToSampleSource();

            SetupSampleSource(source);

            var buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                while (_source.Read(buffer, 0, buffer.Length) > 0);
            };
        }

        private void SetupSampleSource(ISampleSource source)
        {
            const FftSize fftSize = FftSize.Fft4096;
            var spectrumProvider = new SpectrumProvider(
                source.WaveFormat.Channels,
                source.WaveFormat.SampleRate, 
                fftSize);

            _spectrum = new Spectrum.Spectrum(spectrumProvider, Stripper.PixelCount / 2);

            var notificationSource = new SingleBlockNotificationStream(source);
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }

        private void Tick(object state)
        {
            if (!_spectrum.FillFftData())
                return;

            if (_gradient == null)
                return;

            var values = _spectrum.GetSpectrum().ToArray();
            for (int i = 0; i < values.Length; i++)
                values[i] = Math.Min(1, values[i] * _sensivity);

            var currentVolume = values.Sum(x => x);
            _totalVolume = Math.Max(currentVolume, _totalVolume);
            var brightness = (byte) (255 * currentVolume / _totalVolume);

            var colors = new Color[Stripper.PixelCount];
            for (int i = 0; i < values.Length; i++)
            {
                var prev = _gradient.Last(x => x.Value <= values[i]);
                var next = _gradient.First(x => x.Value >= values[i]);

                var d = values[i] - prev.Value;

                var r = (byte)((next.Color.R - prev.Color.R) * d + prev.Color.R);
                var g = (byte)((next.Color.G - prev.Color.G) * d + prev.Color.G);
                var b = (byte)((next.Color.B - prev.Color.B) * d + prev.Color.B);
                var color = Color.FromArgb(r, g, b);

                colors[Stripper.PixelCount / 2 - i - (Stripper.PixelCount / 2) % 2] = color;
                colors[Stripper.PixelCount / 2 + i] = color;
            }

            Stripper.SetPixelsColor(brightness, colors.ToArray());
            RaiseSpectrumUpdated(values, colors, currentVolume / _totalVolume);
        }

        private void RaiseSpectrumUpdated(IEnumerable<double> values, IEnumerable<Color> colors, double brightness)
        {
            SpectrumUpdated?.Invoke(this, new SpectrumUpdatedEventArgs(values, colors, brightness));
        }
    }
}