using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoListViewWithGroups : DemoListView
    {
        public DemoListViewWithGroups(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A4", "B4", "C4" }, "Group 4"),
                new StringGroup(new [] { "A5", "B5", "C5" }, "Group 5"),
            };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));
            IsGroupingEnabled = true;
            GroupDisplayBinding = new Binding(nameof(StringGroup.Title));

            BackgroundColor = Colors.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }
    }
}