using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoButton : Button
    {
        public DemoButton(string text)
        {
            Text = text;

            BackgroundColor = Colors.AliceBlue;
            TextColor = Colors.Black;

            Command = new Command(o => App.ShowMessage("Success", text + " tapped"));
        }
    }
}
