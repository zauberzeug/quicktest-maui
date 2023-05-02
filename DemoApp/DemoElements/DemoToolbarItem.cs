using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class DemoToolbarItem : ToolbarItem
    {
        public DemoToolbarItem()
        {
            Text = "ToolbarItem";
            Command = new Command(o => App.ShowMessage("Success", Text + " tapped"));
        }
    }
}
