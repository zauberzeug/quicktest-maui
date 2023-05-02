using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class DemoCountdown : DemoButton
    {
        public DemoCountdown() : base("Countdown")
        {
            Command = new Command(async o => {
                Text = "3";
                await Task.Delay(1000);
                Text = "2";
                await Task.Delay(1000);
                Text = "1";
                await Task.Delay(1000);
                Text = "0";
                await Task.Delay(1000);
                Text = "Countdown";
            });
        }
    }
}
