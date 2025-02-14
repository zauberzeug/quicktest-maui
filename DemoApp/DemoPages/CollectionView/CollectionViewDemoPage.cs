using Microsoft.Maui.Controls;

namespace DemoApp;

public class CollectionViewDemoPage : ContentPage
{
    public CollectionViewDemoPage()
    {
        Title = "CollectionView demos";

        Content = new StackLayout {
            Children = {
                CreateSubpageButton(new DemoCollectionViewWithLabel() {
                    Header = "plain header", Footer = "plain footer"
                }),
                CreateSubpageButton(new DemoCollectionViewWithDataSelector() {
                    Header = "plain header", Footer = "plain footer"
                }),
                CreateSubpageButton(new DemoCollectionViewWithGrid()),
                CreateSubpageButton(new DemoCollectionViewWithTapAction()),
                CreateSubpageButton(new DemoCollectionViewGrouped()),
            },
        };
    }

    DemoButton CreateSubpageButton(CollectionView collectionView)
    {
        return new DemoButton(collectionView.GetType().Name) {
            Command = new Command(o => Navigation.PushAsync(new NavigationDemoPage(collectionView.GetType().Name) { Content = collectionView })),
        };
    }
}