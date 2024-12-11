using Microsoft.Maui.Controls;

namespace DemoApp;

public class SimpleDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate ViewTemplate { get; set; }
    public DataTemplate EditTemplate { get; set; }

    public SimpleDataTemplateSelector()
    {
    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return (item as string).Contains("Edit") ? EditTemplate : ViewTemplate;
    }
}