using System.Configuration;

namespace StripController.Configuration.ConfigurationSections
{
    public class GradientConfigElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(R), IsRequired = true)]
        public byte R
        {
            get { return (byte)this[nameof(R)]; }
            set { this[nameof(R)] = value; }
        }

        [ConfigurationProperty(nameof(G), IsRequired = true)]
        public byte G
        {
            get { return (byte)this[nameof(G)]; }
            set { this[nameof(G)] = value; }
        }

        [ConfigurationProperty(nameof(B), IsRequired = true)]
        public byte B
        {
            get { return (byte)this[nameof(B)]; }
            set { this[nameof(B)] = value; }
        }

        [ConfigurationProperty(nameof(Offset), IsRequired = true, IsKey = true)]
        public double Offset
        {
            get { return (double)this[nameof(Offset)]; }
            set { this[nameof(Offset)] = value; }
        }

        public GradientConfigElement(byte r, byte g, byte b, double ofset)
        {
            R = r;
            G = g;
            B = b;
            Offset = ofset;
        }

        public GradientConfigElement()
        {
        }
    }
}