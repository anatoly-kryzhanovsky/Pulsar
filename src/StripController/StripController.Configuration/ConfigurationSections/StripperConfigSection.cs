using System.Configuration;
using StripController.Configuration.Interfaces;

namespace StripController.Configuration.ConfigurationSections
{
    public class StripperConfigSection : ConfigurationSection, IStripperSettings
    {
        [ConfigurationProperty(nameof(Address), IsRequired = true)]
        public string Address => (string) this["Address"];

        [ConfigurationProperty(nameof(Port), IsRequired = true)]
        public int Port => (int) this["Port"];

        [ConfigurationProperty(nameof(PixelCount), IsRequired = true)]
        public byte PixelCount => (byte) this["PixelCount"];
    }
}
    
