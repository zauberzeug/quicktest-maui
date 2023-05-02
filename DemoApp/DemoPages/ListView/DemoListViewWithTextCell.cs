using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class DemoListViewWithTextCell : DemoListView
    {
        public DemoListViewWithTextCell(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<string> { "Item A1", "Item B1", "Item C1" };
            ItemTemplate = new DataTemplate(typeof(TextCell));

            BackgroundColor = Colors.GhostWhite;

            ItemTemplate.SetBinding(TextCell.TextProperty, ".");
            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }
    }
}