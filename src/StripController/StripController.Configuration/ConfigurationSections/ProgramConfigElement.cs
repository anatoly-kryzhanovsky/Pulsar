using System;
using System.Configuration;
using StripController.Configuration.Models;

namespace StripController.Configuration.ConfigurationSections
{
    public class ProgramConfigElement : ConfigurationElement
    {
        protected bool Equals(ProgramConfigElement other)
        {
            if (other.ItemType != ItemType)
                return false;

            if (other.Time != Time)
                return false;

            if (ItemType == EProgramItemType.Color)
            {
                if (other.R != R || other.G != G || other.B != B)
                    return false;

                return true;
            }

            if (ItemType == EProgramItemType.Brightness)
            {
                return other.Brightness == Brightness;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProgramConfigElement) obj);
        }

        public override int GetHashCode()
        {
            if (ItemType == EProgramItemType.Color)
            {
                var hash = Time.GetHashCode();
                hash = hash * 23 + R.GetHashCode();
                hash = hash * 23 + G.GetHashCode();
                hash = hash * 23 + B.GetHashCode();
                hash = hash * 23 + PixelStart.GetHashCode();
                hash = hash * 23 + PixelEnd.GetHashCode();

                return hash;
            }

            if (ItemType == EProgramItemType.Brightness)
            {
                var hash = Time.GetHashCode();
                hash = hash * 23 + Brightness.GetHashCode();

                return hash;
            }

            return 0;
        }

        [ConfigurationProperty(nameof(Time), IsRequired = true)]
        public TimeSpan Time 
        {
            get { return (TimeSpan)this[nameof(Time)]; }
            set { this[nameof(Time)] = value; }
        }

        [ConfigurationProperty(nameof(PixelStart), IsRequired = true)]
        public byte PixelStart
        {
            get { return (byte)this[nameof(PixelStart)]; }
            set { this[nameof(PixelStart)] = value; }
        }

        [ConfigurationProperty(nameof(PixelEnd), IsRequired = true)]
        public byte PixelEnd
        {
            get { return (byte)this[nameof(PixelEnd)]; }
            set { this[nameof(PixelEnd)] = value; }
        }

        [ConfigurationProperty(nameof(R), IsRequired = false)]
        public byte R
        {
            get { return (byte)this[nameof(R)]; }
            set { this[nameof(R)] = value; }
        }

        [ConfigurationProperty(nameof(G), IsRequired = false)]
        public byte G
        {
            get { return (byte)this[nameof(G)]; }
            set { this[nameof(G)] = value; }
        }

        [ConfigurationProperty(nameof(B), IsRequired = false)]
        public byte B
        {
            get { return (byte)this[nameof(B)]; }
            set { this[nameof(B)] = value; }
        }

        [ConfigurationProperty(nameof(Brightness), IsRequired = false)]
        public byte Brightness
        {
            get { return (byte)this[nameof(Brightness)]; }
            set { this[nameof(Brightness)] = value; }
        }

        [ConfigurationProperty(nameof(ItemType), IsRequired = true)]
        public EProgramItemType ItemType
        {
            get { return (EProgramItemType)this[nameof(ItemType)]; }
            set { this[nameof(ItemType)] = value; }
        }

        public ProgramConfigElement(byte r, byte g, byte b, TimeSpan time, byte startPixel, byte endPixel)
        {
            R = r;
            G = g;
            B = b;

            PixelStart = startPixel;
            PixelEnd = endPixel;

            Time = time;

            ItemType = EProgramItemType.Color;
        }

        public ProgramConfigElement(byte brigtness, TimeSpan time)
        {
            Brightness = brigtness;
            Time = time;
            ItemType = EProgramItemType.Brightness;
        }

        public ProgramConfigElement()
        {
        }
    }
}