using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StripController.Configuration.Interfaces;
using StripController.Configuration.Models;

namespace StripController.Configuration.ConfigurationSections
{
    public class CaptureModeConfigSection : ConfigurationSection, ICaptureModeSettings
    {
        [ConfigurationProperty(nameof(Sensivity), IsRequired = true)]
        public double Sensivity
        {
            get { return (double) this[nameof(Sensivity)]; }
            set { this[nameof(Sensivity)] = value; }
        }

        [ConfigurationProperty(nameof(Gradient), IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(GradientCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public GradientCollection Gradient
        {
            get { return (GradientCollection) base[nameof(Gradient)]; }
            set { base[nameof(Gradient)] = value; }
        }

        public IReadOnlyCollection<GradientItem> GradientItems
        {
            get
            {
                var list = new List<GradientItem>();
                foreach (GradientConfigElement itm in Gradient)
                    list.Add(new GradientItem {R = itm.R, G = itm.G, B = itm.B, Offset = itm.Offset});

                return list
                    .OrderBy(x => x.Offset)
                    .ToArray();
            }
            set
            {
                Gradient.Clear();
                foreach (var itm in value)
                    Gradient.Add(new GradientConfigElement(itm.R, itm.G, itm.B, itm.Offset));
            }
        }

        public object Clone()
        {
            var gradient = new GradientCollection();
            foreach (GradientConfigElement itm in Gradient)
            {
                gradient.Add(new GradientConfigElement(itm.R, itm.G, itm.B, itm.Offset));
            }

            return new CaptureModeConfigSection
            {
                Sensivity = Sensivity,
                Gradient = gradient
            };
        }
    }
}