using System.Windows;
using System.Windows.Controls;
using StripController.PresentationEntities;

namespace StripController.TemplateSelectors
{
    public class ProgramValueTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ColorDataTemplate { get; set; }
        public DataTemplate BrightnessDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item is ProgramItemPe)
            {
                var programItem = item as ProgramItemPe;

                if (programItem.Type == EProgramItemType.Color)
                    return ColorDataTemplate;

                return BrightnessDataTemplate;
            }

            return null;
        }
    }
}
