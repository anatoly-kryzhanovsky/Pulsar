using System;
using StripController.PresentationEntities;
using StripController.Presenters;

namespace StripController.Services.Modes.Animation
{
    abstract class Animation
    {
        public TimeSpan EndTime { get; }
        public ProgramItemPe Task { get; }

        protected Animation(TimeSpan endTime, ProgramItemPe task)
        {
            EndTime = endTime;
            Task = task;
        }

        public abstract void Apply(StripAnimationState state, bool interpolate, TimeSpan currentTime);
    }
}