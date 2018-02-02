using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StripController.PresentationEntities;

namespace StripController.Services.Modes.Animation
{
    class StripAnimationState
    {
        public List<Animation> ActiveAnimations { get; }
        public PixelState[] Pixels { get; } 
        public double Brightness { get; set; }
        public TimeSpan CurrentTime { get; private set; }

        private ProgramItemPe[] _tasks;
        private bool _interpolate;
        private TimeSpan _frameTime;
        private TimeSpan _totalDuration;
        private int _taskIndex;
        private TimeSpan _cycleStartTime;

        public StripAnimationState(int pixelCount)
        {
            ActiveAnimations = new List<Animation>();
            Pixels = Enumerable
                .Range(0, pixelCount).Select(x => new PixelState())
                .ToArray();
        }

        public void Initialize(ProgramItemPe[] tasks, bool interpolate)
        {
            _tasks = tasks;
            _interpolate = interpolate;

            CurrentTime = TimeSpan.Zero;
            _frameTime = TimeSpan.FromMilliseconds(25);
            _totalDuration = _tasks.OrderBy(x => x.Timeoffset).Last().Timeoffset;
           
            SetInitialState();
        }

        private void SetInitialState()
        {
            ActiveAnimations.Clear();

            _taskIndex = 0;

            for (int pixel = 0; pixel < Pixels.Length; pixel++)
            {
                var initialTask = _tasks.FirstOrDefault(x => x.Type == EProgramItemType.Color && pixel >= x.StartPixel && pixel <= x.EndPixel);

                Pixels[pixel].R = initialTask?.R ?? 255;
                Pixels[pixel].G = initialTask?.G ?? 255;
                Pixels[pixel].B = initialTask?.B ?? 255;
            }

            Brightness = 255;
        }

        public void Spin()
        {
            var elapsedAniamtions = new List<Animation>();

            foreach (var animation in ActiveAnimations)
            {
                animation.Apply(this, _interpolate, CurrentTime);
                if (CurrentTime >= animation.EndTime)
                    elapsedAniamtions.Add(animation);
            }

            foreach (var animation in elapsedAniamtions)
            {
                ActiveAnimations.Remove(animation);
            }

            for (; _taskIndex < _tasks.Length - 1 && _tasks[_taskIndex].Timeoffset + _cycleStartTime <= CurrentTime; _taskIndex++)
            {
                var task = _tasks[_taskIndex];
                if ((task.Timeoffset.TotalMilliseconds <=
                     CurrentTime.TotalMilliseconds % _totalDuration.TotalMilliseconds) &&
                    ActiveAnimations.All(x => x.Task != task))
                {
                    if (task.Type == EProgramItemType.Color)
                    {
                        for (var pixel = task.StartPixel; pixel <= task.EndPixel; pixel++)
                        {
                            var nextTask = _tasks
                                .Skip(_taskIndex + 1)
                                .SkipWhile(x =>
                                    x.Type != EProgramItemType.Color || pixel < x.StartPixel || pixel > x.EndPixel)
                                .OrderBy(x => x.Timeoffset)
                                .FirstOrDefault();

                            if (nextTask == null)
                                continue;

                            ActiveAnimations.Add(new ColorAnimation(
                                pixel,
                                _cycleStartTime + nextTask.Timeoffset,
                                task,
                                Pixels[pixel].R, Pixels[pixel].G, Pixels[pixel].B,
                                nextTask.R, nextTask.G, nextTask.B,
                                _cycleStartTime + nextTask.Timeoffset - CurrentTime,
                                _frameTime));
                        }
                    }
                    else if (task.Type == EProgramItemType.Brightness)
                    {
                        var nextTask = _tasks
                            .Skip(_taskIndex + 1)
                            .SkipWhile(x => x.Type != EProgramItemType.Brightness)
                            .OrderBy(x => x.Timeoffset)
                            .FirstOrDefault();

                        if (nextTask == null)
                            continue;

                        ActiveAnimations.Add(new BrightnessAnimation(
                            _cycleStartTime + nextTask.Timeoffset,
                            task,
                            Brightness,
                            nextTask.Brightness,
                            _cycleStartTime + nextTask.Timeoffset - CurrentTime,
                            _frameTime));
                    }
                }
            }

            Thread.Sleep(_frameTime);
            CurrentTime = CurrentTime.Add(_frameTime);

            if (CurrentTime - _cycleStartTime >= _totalDuration)
            {
                SetInitialState();
                _cycleStartTime = CurrentTime;
            }
        }
    }
}