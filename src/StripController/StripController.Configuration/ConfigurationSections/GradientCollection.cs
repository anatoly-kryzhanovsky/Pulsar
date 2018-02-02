using System.Configuration;

namespace StripController.Configuration.ConfigurationSections
{
    public class GradientCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new GradientConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GradientConfigElement)element).Offset;
        }

        public int IndexOf(GradientConfigElement itm)
        {
            return BaseIndexOf(itm);
        }

        public void Add(GradientConfigElement itm)
        {
            BaseAdd(itm);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(GradientConfigElement itm)
        {
            if (BaseIndexOf(itm) >= 0)
                BaseRemove(itm.Offset);
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