using System.Configuration;
using StripController.Configuration.Interfaces;

namespace StripController.Configuration.ConfigurationSections
{
    public class CustomColorModeConfigSection : ConfigurationSection, ICustomColorModeSettings
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

        [ConfigurationProperty(nameof(AutoApply), IsRequired = true)]
        public bool AutoApply
        {
            get { return (bool)this[nameof(AutoApply)]; }
            set { this[nameof(AutoApply)] = value; }
        }

        public object Clone()
        {
            return new CustomColorModeConfigSection
            {
                R = R,
                G = G,
                B = B,
                AutoApply = AutoApply
            };
        }
    }
}