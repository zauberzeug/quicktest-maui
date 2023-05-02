using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoLabel : Label
    {
        public DemoLabel(string text = null)
        {
            Text = text;
            BackgroundColor = Colors.FloralWhite;
            HorizontalTextAlignment = TextAlignment.Center;
        }

        public DemoLabel WithGestureRecognizer()
        {
            GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => App.ShowMessage("Success", Text + " tapped")),
            });

            return this;
        }

        public DemoLabel Invisible()
        {
            IsVisible = false;

            return this;
        }
    }
}
