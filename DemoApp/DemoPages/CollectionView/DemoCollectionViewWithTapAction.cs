using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoCollectionViewWithTapAction : CollectionView
{
    public DemoCollectionViewWithTapAction()
    {
        ItemsSource = new List<string> { "Item A1", "Item B1", "Item C1" };
        ItemTemplate = new DataTemplate(() => {
            var label = new Label();
            label.SetBinding(Label.TextProperty, ".");

            return new StackLayout {
                Children = { label, new Label() { Text = "Test" } }
            };
        });

        BackgroundColor = Colors.GhostWhite;

        SelectionMode = SelectionMode.Single;
        SelectionChanged += (sender, e) => {
            var item = this.SelectedItem;
            if (item != null)
                Navigation.PushAsync(new NavigationPage(new ContentPage() { Content = new Label { Text = "Page for " + item as string } }));
        };
    }
}