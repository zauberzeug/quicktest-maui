using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class App : Application
    {
        public static string PageLog;
        public static string LifecycleLog;

        public static App Instance;

        public App()
        {
            PageLog = LifecycleLog = "";
            Instance = this;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = new Window();
            ToggleFlyout(window);
            return window;
        }

        public FlyoutPage Flyout { get => Windows[0].Page as FlyoutPage; }

        public void ToggleFlyout(Window window = null)
        {
            if (window == null)
                window = Windows[0];
            
            if (window.Page is FlyoutPage)
                window.Page  = new NavigationPage(new NavigationDemoPage()).AddPageLog();
            else
                window.Page  = new FlyoutPage {
                    Flyout = new MenuPage(),
                    Detail = new NavigationPage(new NavigationDemoPage()).AddPageLog(),
                }.AddPageLog();
        }

        public static void ShowMessage(string title, string message)
        {
            Current.Windows[0].Page.DisplayAlert(title, message, "Ok");
        }

        protected override void OnStart()
        {
            base.OnStart();
            LifecycleLog += "OnStart ";
        }
    }
}
