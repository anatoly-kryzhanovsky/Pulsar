using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StripController.Infrastructure.StripWrapper;
using StripController.PresentationEntities;
using StripController.Services.Modes.Animation;

namespace StripController.Services.Modes
{
    class ProgramMode : IProgramMode
    {
        private readonly TaskScheduler _uiScheduler;
        private Task _playTask;
        private CancellationTokenSource _playCancellationToken;
        private ProgramItemPe[] _programItems;
        private bool _interpolate;
        private bool _isActive;

        public IStripper Stripper { get; }

        public event TimeChangedEventHandler TimeChanged;

        public ProgramMode(IStripper stripper)
        {
            Stripper = stripper;
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void SetProgramItems(IEnumerable<ProgramItemPe> items)
        {
            _programItems = items.OrderBy(x => x.Timeoffset).ToArray();
        }

        public void SetInterpolate(bool interpolate)
        {
            _interpolate = interpolate;
        }
        
        public void Start()
        {
            if (_programItems == null)
                return;

            _playCancellationToken = new CancellationTokenSource();
            _playTask = Task.Run(() => Play());

            _isActive = true;
        }

        public void Stop()
        {
            if (!_isActive)
                return;

            _isActive = false;

            _playCancellationToken.Cancel();
            _playTask.Wait();

            foreach (var item in _programItems)
            {
                item.Current = false;
            }
        }

        private void Play()
        {
            var state = new StripAnimationState(50);
            state.Initialize(_programItems, _interpolate);

            while (!_playCancellationToken.IsCancellationRequested)
            {
                var activeAnimations = state.ActiveAnimations.ToArray();
                Task.Factory.StartNew(() =>
                {
                    foreach (var task in _programItems)
                    {
                        task.Current = false;
                    }

                    foreach (var animation in activeAnimations)
                    {
                        animation.Task.Current = true;
                    }
                }, CancellationToken.None, TaskCreationOptions.None, _uiScheduler);

                state.Spin();
                RaiseTimeChangedEvent(state.CurrentTime);

                Stripper.SetBrightness((byte)state.Brightness);
                Stripper.SetPixelsColor(
                    (byte)state.Brightness, 
                    state.Pixels
                        .Select(x => Color.FromArgb((byte)x.R, (byte)x.G, (byte)x.B))
                        .ToArray());
            }
        }

        private void RaiseTimeChangedEvent(TimeSpan time)
        {
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(time));
        }
    }
}