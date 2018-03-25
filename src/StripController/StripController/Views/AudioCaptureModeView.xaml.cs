using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using StripController.PresentationEntities;
using StripController.ViewInterfaces;

namespace StripController.Views
{
    public partial class AudioCaptureModeView : IAudioCaptureModeView
    {
        public AudioCaptureColorModePe DisplayObject
        {
            get { return (AudioCaptureColorModePe)DataContext; }
            set { DataContext = value; }
        }

        public event SensivityChangedDelegate SensivityChanged;
        public event CaptureStatusChangedDelegate CaptureStatusChanged;
        public event GradientChangedDelegate GradientChanged;
        public event SpectrumImageSizeChangedDelegate SpectrumImageSizeChanged;

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadStateRequested;
        public event EventHandler SaveStateRequested;
        public event EventHandler SaveGradientRequested;
        public event EventHandler LoadGradientRequested;

        public AudioCaptureModeView()
        {
            InitializeComponent();
        }

        public void UpdateVisual()
        {
            using (var ms = new MemoryStream())
            {
                DisplayObject.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();

                DebugImage.Source = bi;
            }
        }

        public void SetGradient(GradientPointPe[] gradient)
        {
            GradientPicker.Gradient = gradient;
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

        public void OnActivated()
        {
        }

        public void OnDeactivated()
        {
        }

        private void RaiseSaveGradientRequestedEvent()
        {
            SaveGradientRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLoadGradientRequestedEvent()
        {
            LoadGradientRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLoadStateRequestedEvent()
        {
            LoadStateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSaveStateRequestedEvent()
        {
            SaveStateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseActivatedEvent()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseDeactivatedEvent()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSensivityChangedEvent(double value)
        {
            SensivityChanged?.Invoke(this, new SensivityChangedArgs(value));
        }

        private void RaiseCaptureStatusChangedEvent(bool isEnabled)
        {
            CaptureStatusChanged?.Invoke(this, new CaptureStatusChangedArgs(isEnabled));
        }

        private void RaiseGradientChangedEvent(IEnumerable<GradientPointPe> gradient)
        {
            GradientChanged?.Invoke(this, new GradientChangedEventArgs(gradient));
        }

        private void OnSensivityChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RaiseSensivityChangedEvent(e.NewValue);
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseCaptureStatusChangedEvent(false);
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseCaptureStatusChangedEvent(true);
        }

        private void GradientPicker_OnGradientChanged(object sender, GradientChangedEventArgs args)
        {
            RaiseGradientChangedEvent(args.Gradient);
        }

        private void RaiseSpectrumImageSizeChangedEvent(int width, int height)
        {
            SpectrumImageSizeChanged?.Invoke(this, new SpectrumImageSizeChangedArgs(width, height));
        }
        
        private void DebugImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RaiseSpectrumImageSizeChangedEvent((int) e.NewSize.Width, (int) e.NewSize.Height);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            RaiseSaveGradientRequestedEvent();
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            RaiseLoadGradientRequestedEvent();
        }
    }
}
