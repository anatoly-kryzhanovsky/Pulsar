using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StripController.Configuration.Interfaces;
using StripController.Configuration.Models;

namespace StripController.Configuration.ConfigurationSections
{
    public class ProgramModeConfigurationSection : ConfigurationSection, IProgramModeSettings
    {
        [ConfigurationProperty(nameof(Program), IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProgramCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ProgramCollection Program
        {
            get { return (ProgramCollection)base[nameof(Program)]; }
            set { base[nameof(Program)] = value; }
        }

        [ConfigurationProperty(nameof(Interpolate), IsRequired = true)]
        public bool Interpolate
        {
            get { return (bool)this[nameof(Interpolate)]; }
            set { this[nameof(Interpolate)] = value; }
        }

        public IReadOnlyCollection<ProgramItem> ProgramItems
        {
            get
            {
                var list = new List<ProgramItem>();
                foreach (ProgramConfigElement itm in Program)
                    list.Add(new ProgramItem
                    {
                        B = itm.B,
                        G = itm.G,
                        R = itm.R,
                        Brightness = itm.Brightness,
                        Time = itm.Time,
                        Type = itm.ItemType,
                        StartPixel = itm.PixelStart,
                        EndPixel = itm.PixelEnd
                    });

                return list
                    .OrderBy(x => x.Time)
                    .ToArray();
            }
            set
            {
                Program.Clear();
                foreach (var itm in value)
                {
                    if (itm.Type == EProgramItemType.Color)
                        Program.Add(new ProgramConfigElement(itm.R, itm.G, itm.B, itm.Time, itm.StartPixel, itm.EndPixel));

                    if (itm.Type == EProgramItemType.Brightness)
                        Program.Add(new ProgramConfigElement(itm.Brightness, itm.Time));
                }
            }
        }

        public object Clone()
        {
            var program = new ProgramCollection();
            foreach (ProgramConfigElement itm in Program)
            {
                if(itm.ItemType == EProgramItemType.Brightness)
                    program.Add(new ProgramConfigElement(itm.Brightness, itm.Time));

                if (itm.ItemType == EProgramItemType.Color)
                    program.Add(new ProgramConfigElement(itm.R, itm.G, itm.B, itm.Time, itm.PixelStart, itm.PixelEnd));
            }

            return new ProgramModeConfigurationSection
            {
                Interpolate = Interpolate,
                Program = program
            };
        }
    }
}