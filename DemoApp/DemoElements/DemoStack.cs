using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoStack : StackLayout
    {
        public DemoStack()
        {
            Children.Add(new DemoLabel("label in tap-able layout"));
            AutomationId = "tappable-stack";

            BackgroundColor = Colors.Gray.MultiplyAlpha(0.2f);
            Padding = 10;

            GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => App.ShowMessage("Success", "StackLayout tapped")),
            });
        }
    }
}
