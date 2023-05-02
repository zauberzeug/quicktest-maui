﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class TabbedTitleViewPage : TabbedPage
    {
        public TabbedTitleViewPage()
        {
            Title = "TabbedTitleViewPage";

            Children.Add(CreateTitleViewPage());
            Children.Add(CreateContentPage());
        }

        TitleViewPage CreateTitleViewPage()
        {
            var titleViewPage = new TitleViewPage();
            titleViewPage.Appearing += (s, e) => NavigationPage.SetTitleView(this, titleViewPage.TitleView);
            titleViewPage.Disappearing += (s, e) => NavigationPage.SetTitleView(this, null);
            return titleViewPage;
        }

        ContentPage CreateContentPage()
        {
            return new ContentPage {
                Title = "ContentPage",
                Content = new Label { Text = "Some content" }
            };
        }
    }
}
