using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class DemoEntry : Entry
    {
        public DemoEntry(string automationId, string placeholder = null, string text = null)
        {
            AutomationId = automationId;
            Placeholder = placeholder;
            Text = text;
        }
    }
}
