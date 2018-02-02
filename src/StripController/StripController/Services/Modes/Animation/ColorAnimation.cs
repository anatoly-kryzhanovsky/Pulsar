using System;
using StripController.PresentationEntities;
using StripController.Presenters;

namespace StripController.Services.Modes.Animation
{
    class ColorAnimation: Animation
    {
        public byte Pixel { get; }

        public double DeltaR { get; }
        public double DeltaG { get; }
        public double DeltaB { get; }

        public byte TargetR { get; }
        public byte TargetG { get; }
        public byte TargetB { get; }

        public ColorAnimation(
            byte pixel, 
            TimeSpan endTime,
            ProgramItemPe task,
            double currentR, double currentG, double currentB, 
            byte targetR, byte targetG, byte targetB, 
            TimeSpan duration, TimeSpan frameTime)
            :base(endTime, task)
        {
            Pixel = pixel;

            DeltaR = (targetR - currentR) / duration.TotalMilliseconds * frameTime.TotalMilliseconds;
            DeltaG = (targetG - currentG) / duration.TotalMilliseconds * frameTime.TotalMilliseconds;
            DeltaB = (targetB - currentB) / duration.TotalMilliseconds * frameTime.TotalMilliseconds;

            TargetR = targetR;
            TargetG = targetG;
            TargetB = targetB;
        }

        public override void Apply(StripAnimationState state, bool interpolate, TimeSpan currentTime)
        {
            if (interpolate)
            {
                state.Pixels[Pixel].R += DeltaR;
                state.Pixels[Pixel].G += DeltaG;
                state.Pixels[Pixel].B += DeltaB;
            }
            else if (EndTime <= currentTime)
            {
                state.Pixels[Pixel].R = TargetR;
                state.Pixels[Pixel].G = TargetG;
                state.Pixels[Pixel].B = TargetB;
            }
        }
    }
}