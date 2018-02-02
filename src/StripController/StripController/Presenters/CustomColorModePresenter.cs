using System;
using StripController.Configuration.Interfaces;
using StripController.Infrastructure.StripWrapper;
using StripController.PresentationEntities;
using StripController.Services.Modes;
using StripController.ViewInterfaces;

namespace StripController.Presenters
{
    class CustomColorModePresenter
    {
        private readonly ICustomColorModeView _view;
        private readonly ICustomColorModeSettings _settings;
        private readonly ICustomColorMode _mode;

        public CustomColorModePresenter(
            ICustomColorModeView view,
            ICustomColorModeSettings settings,
            IStripper stripper)
        {
            _view = view;
            _settings = settings;

            _view.DisplayObject = new CustomColorModePe {Brightness = 255, AutoApply = true};
            _view.ColorChanged += OnColorChanged;
            _view.ApplyColor += OnApply;
            _view.BrightnessChanged += ViewOnBrightnessChanged;

            _view.Activated += ViewOnActivated;
            _view.Deactivated += ViewOnDeactivated;
            _view.LoadStateRequested += ViewOnLoadStateRequested;
            _view.SaveStateRequested += ViewOnSaveStateRequested;

            _mode = new CustomColorMode(stripper);
        }

        private void ViewOnSaveStateRequested(object sender, EventArgs eventArgs)
        {
            _settings.R = _view.DisplayObject.R;
            _settings.G = _view.DisplayObject.G;
            _settings.B = _view.DisplayObject.B;
            _settings.AutoApply = _view.DisplayObject.AutoApply;
        }

        private void ViewOnLoadStateRequested(object sender, EventArgs eventArgs)
        {
            _view.DisplayObject.R = _settings.R;
            _view.DisplayObject.G = _settings.G;
            _view.DisplayObject.B = _settings.B;
            _view.DisplayObject.AutoApply = _settings.AutoApply;
        }

        private void ViewOnDeactivated(object sender, EventArgs eventArgs)
        {
            _mode.Stop();
        }

        private void ViewOnActivated(object sender, EventArgs eventArgs)
        {
        }

        private void ViewOnBrightnessChanged(object sender, BrightnessChangedArgs args)
        {
            if (_view.DisplayObject.AutoApply)
                ApplySelectedColor();
        }

        private void OnApply(object sender, EventArgs args)
        {
            ApplySelectedColor();
        }

        private void OnColorChanged(object sender, ColorChangedArgs args)
        {
            _view.DisplayObject.R = args.R;
            _view.DisplayObject.G = args.G;
            _view.DisplayObject.B = args.B;

            if (_view.DisplayObject.AutoApply)
                ApplySelectedColor();
        }

        private void ApplySelectedColor()
        {
            _mode.SetBrigntness(_view.DisplayObject.Brightness);
            _mode.SetColor(_view.DisplayObject.R, _view.DisplayObject.G, _view.DisplayObject.B);
        }
    }
}
