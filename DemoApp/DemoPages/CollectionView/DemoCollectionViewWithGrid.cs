using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoCollectionViewWithGrid : CollectionView
{
    public DemoCollectionViewWithGrid()
    {
        ItemsSource = new List<string> { "Some Data" };
        ItemTemplate = new DataTemplate(() => {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Add(new Label { Text = "Col 1" }, 0, 0);
            grid.Add(new Label { Text = "Col 2" }, 1, 0);

            return grid;
        });

        BackgroundColor = Colors.GhostWhite;
    }
}