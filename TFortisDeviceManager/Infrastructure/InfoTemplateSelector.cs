using System.Windows;
using System.Windows.Controls;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Infrastructure
{
    public class InfoTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate InfoTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ISettingsState)
                return InfoTemplate;

            return EmptyTemplate;            
        }
    }
}
