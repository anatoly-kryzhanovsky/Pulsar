using System;
using System.Collections.ObjectModel;
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
using EProgramItemType = StripController.PresentationEntities.EProgramItemType;

namespace StripController.Presenters
{
    class ProgramModePresenter
    {
        private readonly IViewFactory _viewFactory;
        private readonly IProgramModeSettings _settings;
        private readonly IProgramModeView _view;
        private readonly IProgramMode _mode;
        private readonly TaskScheduler _uiScheduler;

        public ProgramModePresenter(
            IViewFactory viewFactory,
            IProgramModeSettings settings, 
            IProgramModeView view,
            IStripper stripper)
        {
            _viewFactory = viewFactory;
            _settings = settings;
            _view = view;
            _view.DisplayObject = new ProgramModePe
            {
                CanPause = false,
                CanPlay = true,
                CanStop = false,
                Interpolate = true
            };

            _view.DeleteItemRequested += ViewOnDeleteItemRequested;
            _view.CreateItemRequested += ViewOnCreateItemRequested;
            _view.PlayRequested += ViewOnPlayRequested;
            _view.StopRequested += ViewOnStopRequested;
            _view.SaveProgramRequested += ViewOnSaveProgramRequested;
            _view.LoadProgramRequested += ViewOnLoadProgramRequested;
            _view.ResetProgramRequested += ViewOnResetProgramRequested;

            _view.Activated += ViewOnActivated;
            _view.Deactivated += ViewOnDeactivated;
            _view.SaveStateRequested += ViewOnSaveStateRequested;
            _view.LoadStateRequested += ViewOnLoadStateRequested;

            _mode = new ProgramMode(stripper);
            _mode.TimeChanged += ModeOnTimeChanged;

            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void ViewOnLoadStateRequested(object sender, EventArgs eventArgs)
        {
            _view.DisplayObject.Interpolate = _settings.Interpolate;
            _view.DisplayObject.Items = new ObservableCollection<ProgramItemPe>(_settings.ProgramItems
                .Select(x => new ProgramItemPe
                {
                    Timeoffset = x.Time,
                    B = x.B,
                    G = x.G,
                    R = x.R,
                    Brightness = x.Brightness,
                    Current = false,
                    EndPixel = x.EndPixel,
                    StartPixel = x.StartPixel,
                    Type = x.Type == Configuration.Models.EProgramItemType.Brightness
                        ? EProgramItemType.Brightness
                        : EProgramItemType.Color
                }));
        }

        private void ViewOnSaveStateRequested(object sender, EventArgs eventArgs)
        {
            _settings.Interpolate = _view.DisplayObject.Interpolate;
            _settings.ProgramItems = _view.DisplayObject.Items
                .Select(x => new ProgramItem
                {
                    B = x.B,
                    G = x.G,
                    R = x.R,
                    Brightness = x.Brightness,
                    EndPixel = x.EndPixel,
                    StartPixel = x.StartPixel,
                    Time = x.Timeoffset,
                    Type = x.Type == EProgramItemType.Brightness
                        ? Configuration.Models.EProgramItemType.Brightness
                        : Configuration.Models.EProgramItemType.Color
                })
                .ToArray();
        }

        private void ViewOnDeactivated(object sender, EventArgs eventArgs)
        {
            _mode.Stop();
        }

        private void ViewOnActivated(object sender, EventArgs eventArgs)
        {
        }

        private void ModeOnTimeChanged(object sender, TimeChangedEventArgs args)
        {
            Task.Factory.StartNew(() =>
            {
                _view.DisplayObject.CurrentTime = args.Time;
            }, CancellationToken.None, TaskCreationOptions.None, _uiScheduler);
        }

        private void ViewOnResetProgramRequested(object sender, EventArgs args)
        {
            _view.DisplayObject.Items.Clear();
        }

        private void ViewOnLoadProgramRequested(object sender, EventArgs args)
        {
            var dialog = _viewFactory.CreateOpenFileDialog();
            dialog.Filter = "Strip programs(*.json)|*.json" + "|All files (*.*)|*.* ";
            if (!dialog.ShowDialog())
                return;

            var rawData = File.ReadAllText(dialog.FileName);
            _view.DisplayObject = JsonConvert.DeserializeObject<ProgramModePe>(rawData);
        }

        private void ViewOnSaveProgramRequested(object sender, EventArgs args)
        {
            var dialog = _viewFactory.CreateSaveFileDialog();
            dialog.Filter = "Strip programs(*.json)|*.json" + "|All files (*.*)|*.* ";
            if (!dialog.ShowDialog())
                return;

            var rawData = JsonConvert.SerializeObject(_view.DisplayObject);
            File.WriteAllText(dialog.FileName, rawData);
        }

        private void ViewOnStopRequested(object sender, EventArgs args)
        {
            _view.DisplayObject.CanPlay = true;
            _view.DisplayObject.CanStop = false;
            _view.DisplayObject.CanPause = false;

            _mode.Stop();
        }
        
        private void ViewOnPlayRequested(object sender, EventArgs args)
        {
            _view.DisplayObject.CanPlay = false;
            _view.DisplayObject.CanStop = true;
            _view.DisplayObject.CanPause = true;

            _mode.SetInterpolate(_view.DisplayObject.Interpolate);
            _mode.SetProgramItems(_view.DisplayObject.Items);
            _mode.Start();
        }
        
        private void ViewOnCreateItemRequested(object sender, EventArgs args)
        {
            _view.DisplayObject.Items.Add(new ProgramItemPe
            {
                R = 255,
                G = 255,
                B = 255,
                Brightness = 255,
                StartPixel = 0,
                EndPixel = (byte)(_mode.Stripper.PixelCount - 1),
                Type = EProgramItemType.Color,
                Timeoffset = _view.DisplayObject.Items.Count == 0
                    ? TimeSpan.FromMilliseconds(0)
                    : _view.DisplayObject.Items.Max(x => x.Timeoffset).Add(TimeSpan.FromSeconds(1))
            });
        }

        private void ViewOnDeleteItemRequested(object sender, DeleteItemRequestedAgrs args)
        {
            _view.DisplayObject.Items.Remove(args.Item);
        }
    }
}