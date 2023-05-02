using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace DemoApp.Droid;

[Activity(
    Label = nameof(DemoApp),
    Icon = "@drawable/icon",
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}

