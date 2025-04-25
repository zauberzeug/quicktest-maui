using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoRadioButton : RadioButton
{
    public DemoRadioButton(string text)
    {
        Content = text;

        BackgroundColor = Colors.AliceBlue;
        TextColor = Colors.Black;

        CheckedChanged += (s, e) => {
            if (e.Value)
                App.ShowMessage("Success", text + " checked");
            else
                App.ShowMessage("Success", text + " unchecked");
        };
    }
}