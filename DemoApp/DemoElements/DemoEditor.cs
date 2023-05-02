using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class DemoEditor : Editor
    {
        public DemoEditor(string automationId, string text = null)
        {
            AutomationId = automationId;
            Text = text;
        }
    }
}
