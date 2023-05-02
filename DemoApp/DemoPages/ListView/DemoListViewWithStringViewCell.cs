using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoListViewWithStringViewCell : DemoListView
    {
        public DemoListViewWithStringViewCell(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<string> { "Item A2", "Item B2", "Item C2" };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));

            BackgroundColor = Colors.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }

        class StringDemoCell : ViewCell
        {
            public StringDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, ".");
                View = label;
            }
        }
    }
}