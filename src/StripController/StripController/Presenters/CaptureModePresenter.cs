using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StripController.Configuration.Interfaces;
using StripController.Configuration.Models;
using StripController.Infrastructure.StripWrapper;
using StripController.PresentationEntities;
using StripController.Services;
using StripController.Services.Modes;
using StripController.ViewInterfaces;
using StripController.Views;

namespace StripController.Presenters
{
    class CaptureModePresenter
    {
        private readonly IAudioCaptureModeView _view;
        private readonly IViewFactory _viewFactory;
        private readonly ICaptureModeSettings _settings;
        private readonly TaskScheduler _uiScheduler;
        
        private readonly ICaptureMode _mode;
        private IEnumerable<GradientPointPe> _gradient;

        public CaptureModePresenter(
            IAudioCaptureModeView view,
            IViewFactory viewFactory,
            ICaptureModeSettings settings, 
            IStripper stripper)
        {
            _view = view;
            _viewFactory = viewFactory;
            _settings = settings;
            _view.DisplayObject = new CaptureColorModePe
            {
                Sensivity = 1,
                Bitmap = new Bitmap(350, 350)
            };
            
            _view.SensivityChanged += ViewOnSensivityChanged;
            _view.CaptureStatusChanged += ViewOnCaptureStatusChanged;
            _view.GradientChanged += ViewOnGradientChanged;
            _view.SpectrumImageSizeChanged += ViewOnSpectrumImageSizeChanged;
            _view.LoadGradientRequested += ViewOnLoadGradientRequested;
            _view.SaveGradientRequested += ViewOnSaveGradientRequested;

            _view.Activated += ViewOnActivated;
            _view.Deactivated += ViewOnDeactivated;
            _view.LoadStateRequested += ViewOnLoadStateRequested;
            _view.SaveStateRequested += ViewOnSaveStateRequested;

            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _mode = new CaptureMode(stripper);
            _mode.SpectrumUpdated += ModeOnSpectrumUpdated;
        }

        private void ViewOnSaveGradientRequested(object sender, EventArgs eventArgs)
        {
            var dialog = _viewFactory.CreateSaveFileDialog();
            dialog.Filter = "Gradient(*.json)|*.json" + "|All files (*.*)|*.* ";
            if (!dialog.ShowDialog())
                return;

            var rawData = JsonConvert.SerializeObject(_gradient.Select(x => new GradientPointPe
            {
                Color = x.Color,
                Value = 1 - x.Value
            }));
            File.WriteAllText(dialog.FileName, rawData);
        }

        private void ViewOnLoadGradientRequested(object sender, EventArgs eventArgs)
        {
            var dialog = _viewFactory.CreateOpenFileDialog();
            dialog.Filter = "Gradient(*.json)|*.json" + "|All files (*.*)|*.* ";
            if (!dialog.ShowDialog())
                return;

            var rawData = File.ReadAllText(dialog.FileName);
            _gradient = JsonConvert.DeserializeObject<IEnumerable<GradientPointPe>>(rawData);
            _view.SetGradient(_gradient.ToArray());
        }

        private void ViewOnSaveStateRequested(object sender, EventArgs eventArgs)
        {
            _settings.Sensivity = _view.DisplayObject.Sensivity;
            _settings.GradientItems = _gradient
                .Select(x => new GradientItem
                {
                    R = x.Color.R,
                    G = x.Color.G,
                    B = x.Color.B,
                    Offset = 1 - x.Value
                })
                .ToArray();
        }

        private void ViewOnLoadStateRequested(object sender, EventArgs eventArgs)
        {
            _view.DisplayObject.Sensivity = _settings.Sensivity;
            _view.SetGradient(_settings.GradientItems
                .Select(x => new GradientPointPe
                {
                    Value = x.Offset,
                    Color = System.Windows.Media.Color.FromRgb(x.R, x.G, x.B)
                })
                .ToArray());
        }

        private void ViewOnSpectrumImageSizeChanged(object sender, SpectrumImageSizeChangedArgs args)
        {
            if(args.Width != _view.DisplayObject.Bitmap.Width || args.Height != _view.DisplayObject.Bitmap.Height)
                _view.DisplayObject.Bitmap = new Bitmap(args.Width, args.Height);
        }

        private void ModeOnSpectrumUpdated(object sender, SpectrumUpdatedEventArgs args)
        {
            Task.Factory.StartNew(() =>
                {
                    CreateSpectrumLine(_view.DisplayObject.Bitmap.Size, args.Values, args.Colors, args.Brightness);
                    _view.UpdateVisual();
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                _uiScheduler);
        }

        private void ViewOnDeactivated(object sender, EventArgs eventArgs)
        {
            _mode.Stop();
        }

        private void ViewOnActivated(object sender, EventArgs eventArgs)
        {
        }
        
        private void ViewOnGradientChanged(object sender, GradientChangedEventArgs args)
        {
            _gradient = args.Gradient
                .OrderBy(x => x.Value)
                .Select(x => new GradientPointPe
                    {
                        Color = x.Color,
                        Value = 1 - x.Value
                    })
                .Reverse()
                .ToArray();
            _mode.SetGradient(args.Gradient);
        }

        private void ViewOnCaptureStatusChanged(object sender, CaptureStatusChangedArgs args)
        {
            _view.DisplayObject.IsEnabled = args.IsEnabled;

            if(_view.DisplayObject.IsEnabled)
                _mode.Start();
            else
                _mode.Stop();
        }

        private void ViewOnSensivityChanged(object sender, SensivityChangedArgs args)
        {
            _mode.SetSensivite(args.Value);
        }

        private void CreateSpectrumLine(Size size, IEnumerable<double> values, IEnumerable<Color> colors, double brightness)
        {
            using (Graphics graphics = Graphics.FromImage(_view.DisplayObject.Bitmap))
            {
                graphics.Clear(Color.Black);
                CreateSpectrumLineInternal(graphics, size, values, colors, brightness);
            }
        }

        private void CreateSpectrumLineInternal(Graphics graphics, Size size, IEnumerable<double> values, IEnumerable<Color> colors, double brightness)
        {
            var barWidth = size.Width / (_mode.Stripper.PixelCount * 2);
            var height = size.Height - barWidth;
            var fftValues = values.ToArray();
            var colors1 = colors.ToArray();

            for (int i = 0; i < fftValues.Length; i++)
            {
                var xCoord1 = size.Width / 2 - (_mode.Stripper.PixelCount / 2) % 2 - i * barWidth * 2;
                var xCoord2 = size.Width / 2 + i * barWidth * 2;

                using (var brush = new LinearGradientBrush(new PointF(0, 0), new PointF(barWidth, height), Color.Black, Color.Black))
                {
                    var cb = new ColorBlend
                    {
                        Positions = _gradient.Select(x => (float) x.Value).ToArray(),
                        Colors = _gradient.Select(x => Color.FromArgb((byte)(x.Color.R * brightness), (byte)(x.Color.G * brightness), (byte)(x.Color.B * brightness))).ToArray()
                    };
                    brush.InterpolationColors = cb;
                    
                    graphics.DrawLine(
                        new Pen(brush, barWidth),
                        new PointF(xCoord1, height),
                        new PointF(xCoord1, height - (float)fftValues[i] * height - 1));

                    graphics.DrawLine(
                        new Pen(brush, barWidth),
                        new PointF(xCoord2, height),
                        new PointF(xCoord2, height - (float)fftValues[i] * height - 1));
                }

                var r = (byte)(colors1[i].R * brightness);
                var g = (byte)(colors1[i].G * brightness);
                var b = (byte)(colors1[i].B * brightness);

                graphics.FillEllipse(new SolidBrush(Color.FromArgb(r, g, b)), xCoord1 - barWidth / 2, barWidth, barWidth, barWidth);
                graphics.FillEllipse(new SolidBrush(Color.FromArgb(r, g, b)), xCoord2 - barWidth / 2, barWidth, barWidth, barWidth);
            }
        }
    }
}