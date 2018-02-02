using System.Configuration;

namespace StripController.Configuration.ConfigurationSections
{
    public class ProgramCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProgramConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
           return (ProgramConfigElement)element;
        }

        public int IndexOf(ProgramConfigElement itm)
        {
            return BaseIndexOf(itm);
        }

        public void Add(ProgramConfigElement itm)
        {
            BaseAdd(itm);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ProgramConfigElement itm)
        {
            if (BaseIndexOf(itm) >= 0)
                BaseRemove(GetElementKey(itm));
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}