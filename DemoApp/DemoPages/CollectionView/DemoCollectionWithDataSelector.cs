using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoCollectionViewWithDataSelector : CollectionView
{
    public DemoCollectionViewWithDataSelector()
    {
        var selector = new SimpleDataTemplateSelector {
            EditTemplate = new DataTemplate(() => {
                var label = new Label();
                label.Text = "Edit Me!";
                label.BackgroundColor = Colors.Red;
                return label;
            }),
            ViewTemplate = new DataTemplate(() => {
                var label = new Label();
                label.Text = "Read Me!";
                label.BackgroundColor = Colors.LightGreen;
                return label;
            })
        };
        ItemsSource = new List<string> { "Edit Me !", "Read Me!", "Edit Me, Too!" };
        ItemTemplate = selector;

        BackgroundColor = Colors.GhostWhite;
    }
}