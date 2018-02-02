using System;
using StripController.PresentationEntities;
using StripController.Presenters;

namespace StripController.Services.Modes.Animation
{
    class BrightnessAnimation: Animation
    {
        public double Delta { get; }
        public byte TargetValue { get; }

        public BrightnessAnimation(
            TimeSpan endTime,
            ProgramItemPe task,
            double currentValue,
            byte targetValue,
            TimeSpan duration,
            TimeSpan frameTime)
            :base(endTime, task)
        {
            Delta = (targetValue - currentValue) / duration.TotalMilliseconds * frameTime.TotalMilliseconds;
            TargetValue = targetValue;
        }

        public override void Apply(StripAnimationState state, bool interpolate, TimeSpan currentTime)
        {
            if(interpolate)
                state.Brightness += (byte) Delta;

            else if (EndTime <= currentTime)
                state.Brightness = TargetValue;
        }
    }
}