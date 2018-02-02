using StripController.PresentationEntities;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StripController.ViewInterfaces;

namespace StripController.Views
{
    public partial class CustomColorModeView : ICustomColorModeView
    {
        private Bitmap _bmp;
        private bool _isPressed;
        private bool _isInitialized;

        public CustomColorModePe DisplayObject
        {
            get { return (CustomColorModePe)DataContext; }
            set { DataContext = value; }
        }

        public event ColorChangedDelegate ColorChanged;
        public event ApplyColorDelegate ApplyColor;
        public event BrightnessChangedDeletage BrightnessChanged;

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadStateRequested;
        public event EventHandler SaveStateRequested;

        public CustomColorModeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public void Activate()
        {
            RaiseActivatedEvent();
        }

        public void Deactivate()
        {
            RaiseDeactivatedEvent();
        }

        public void LoadState()
        {
            RaiseLoadStateRequestedEvent();
        }

        public void SaveState()
        {
            RaiseSaveStateRequestedEvent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);


            if (!_isInitialized)
            {
                _isInitialized = true;

                for (int y = 0; y < _bmp.Height; y++)
                for (int x = 0; x < _bmp.Width; x++)
                {
                    var pixel = _bmp.GetPixel(x, y);
                    if (pixel.R == DisplayObject.R &&
                        pixel.G == DisplayObject.G &&
                        pixel.B == DisplayObject.B)
                    {
                        var scledX = Pallate.ActualWidth / _bmp.Width * x;
                        var scledY = Pallate.ActualHeight / _bmp.Height * y;

                        var offset = Pallate.TranslatePoint(new System.Windows.Point(0, 0), this);
                        Target.RenderTransform = new TranslateTransform(offset.X + scledX - Target.ActualWidth / 2, offset.Y + scledY - Target.ActualHeight / 2);
                        Target.Visibility = Visibility.Visible;

                        break;
                    }
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ms = new MemoryStream();
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Pallate.Source as BitmapSource));
            encoder.Save(ms);
            ms.Flush();

            _bmp = new Bitmap(ms);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;
            Mouse.OverrideCursor = Cursors.None;
            Target.Visibility = Visibility.Visible;
            RaiseColorChangedEvent();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if(_isPressed)
                RaiseColorChangedEvent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseApplyColorEvent();
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RaiseBrightnessChangedEvent();
        }

        private void RaiseActivatedEvent()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseDeactivatedEvent()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseColorChangedEvent()
        {
            var position = Mouse.GetPosition(Pallate);
            var scledX = _bmp.Width / Pallate.ActualWidth * position.X;
            var scledY = _bmp.Height / Pallate.ActualHeight * position.Y;

            var color = _bmp.GetPixel(Math.Min((int)scledX, _bmp.Width - 1), Math.Min((int)scledY, _bmp.Height - 1));

            ColorChanged?.Invoke(this, new ColorChangedArgs(color.R, color.G, color.B));

            var offset = Pallate.TranslatePoint(new System.Windows.Point(0, 0), this);
            Target.RenderTransform = new TranslateTransform(offset.X + position.X - Target.ActualWidth / 2, offset.Y + position.Y - Target.ActualHeight / 2);
        }

        private void RaiseApplyColorEvent()
        {
            ApplyColor?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseBrightnessChangedEvent()
        {
            BrightnessChanged?.Invoke(this, new BrightnessChangedArgs((byte)Brightness.Value));
        }

        private void RaiseLoadStateRequestedEvent()
        {
            LoadStateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSaveStateRequestedEvent()
        {
            SaveStateRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
