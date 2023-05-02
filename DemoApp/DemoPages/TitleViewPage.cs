﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class TitleViewPage : ContentPage
    {
        Label infoLabel;

        public TitleViewPage()
        {
            Title = "TitleViewPage";

            infoLabel = new Label();

            ToolbarItems.Add(new DemoToolbarItem());

            TitleView = CreateTitelView();
            NavigationPage.SetTitleView(this, TitleView);

            Content = new StackLayout {
                Margin = 20,
                Children = { infoLabel }
            };
        }

        public StackLayout TitleView { get; private set; }

        StackLayout CreateTitelView()
        {
            return new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = {
                    CreateTitleViewLabel(),
                    CreateTitleViewButton(),
                }
            };
        }

        Label CreateTitleViewLabel()
        {
            var titelViewLabel = new Label {
                Text = "TitleViewLabel",
                VerticalOptions = LayoutOptions.Center
            };
            titelViewLabel.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(OnTitleViewLabelTapped) });
            return titelViewLabel;
        }

        Button CreateTitleViewButton()
        {
            return new Button {
                Text = "TitleViewButton",
                Command = new Command(OnTitleViewButtonTapped),
            };
        }

        void OnTitleViewLabelTapped() => infoLabel.Text = "Tapped on Label";

        void OnTitleViewButtonTapped() => infoLabel.Text = "Tapped on Button";
    }
}
