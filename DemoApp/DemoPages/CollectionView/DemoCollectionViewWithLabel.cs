using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoCollectionViewWithLabel : CollectionView
{
    public DemoCollectionViewWithLabel()
    {
        ItemsSource = new List<string> { "Item A1", "Item B1", "Item C1" };
        ItemTemplate = new DataTemplate(typeof(Label));

        BackgroundColor = Colors.GhostWhite;

        ItemTemplate.SetBinding(Label.TextProperty, ".");
    }
}