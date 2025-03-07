using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public static class PageExtensions
    {
        public static T AddPageLog<T>(this T page) where T : Page => AddPageLog(page, (s) => App.PageLog += s);
        
        public static T AddPageLog<T>(this T page, Action<string> addToLog) where T : Page
        {
            page.Appearing += (s, e) => OnAppearing(s, e, addToLog);
            page.Disappearing += (s, e) => OnDisappearing(s, e, addToLog);
            return page;
        }

        static void OnAppearing(object sender, EventArgs e, Action<string> addToLog)
        {
            var page = sender as Page;
            var logMessage = $"A({GetLogName(page)})";
            addToLog($"{logMessage} ");
            Console.WriteLine(logMessage);
        }

        static void OnDisappearing(object sender, EventArgs e, Action<string> addToLog)
        {
            var page = sender as Page;
            var logMessage = $"D({GetLogName(page)})";
            addToLog($"{logMessage} ");
            Console.WriteLine(logMessage);
        }

        static string GetLogName(Page page) => page.Title ?? page.GetType().Name;
    }
}
