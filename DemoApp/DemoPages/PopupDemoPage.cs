﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class PopupDemoPage : ContentPage
    {
        Label result;

        public PopupDemoPage()
        {
            Title = "Popup demo";

            Content = new StackLayout {
                Children = {
                    new Button {
                        Text = "Show yes/no alert",
                        Command = new Command(ShowYesNoAlert),
                    },
                    new Button {
                        Text = "Show ok alert",
                        Command = new Command(ShowOkAlert),
                    },
                    new Button {
                        Text = "Show ok alert with repeated text",
                        Command = new Command(ShowOkAlertWithRepeatedText),
                    },
                    new Button {
                        Text = "Show action sheet",
                        Command = new Command(ShowActionSheet),
                    },
                    new Button {
                        Text = "Show action sheet without cancel",
                        Command = new Command(ShowActionSheetWithoutCancel),
                    },
                    new Button {
                        Text = "Show action sheet without destruction",
                        Command = new Command(ShowActionSheetWithoutDestruction),
                    },
                    new Button {
                        Text = "Show action sheet with repeated text",
                        Command = new Command(ShowActionSheetWithRepeatedText),
                    },
                    new Button {
                        Text = "Show action sheet which triggers alert",
                        Command = new Command(ShowActionSheetWhichTriggersAlert),
                    },
                    (result = new Label()),
                    new Label() {
                        Text = "Some text",
                    },
                    new Label() {
                        Text = "Some duplicated text",
                    },
                    new Label() {
                        Text = "Some duplicated text",
                    },
                }
            };
        }

        async void ShowYesNoAlert()
        {
            var result = await DisplayAlert("Alert", "Message", "Yes", "No");
            this.result.Text = $"Alert result: {result}";
        }

        async void ShowOkAlert()
        {
            await DisplayAlert("Alert", "Message", "Ok");
            result.Text = $"Alert result: Ok";
        }

        async void ShowOkAlertWithRepeatedText()
        {
            await DisplayAlert("Message", "Message", "Message");
            result.Text = $"Alert result: Message";
        }

        async void ShowActionSheet()
        {
            var result = await DisplayActionSheet("Action sheet", "Cancel", "Destroy", "Option 1", "Option 2");
            this.result.Text = $"Action sheet result: {result}";
        }

        async void ShowActionSheetWithoutCancel()
        {
            var result = await DisplayActionSheet("Action sheet without cancel", null, "Destroy", "Option 1");
            this.result.Text = $"Action sheet without cancel: {result}";
        }

        async void ShowActionSheetWithoutDestruction()
        {
            var result = await DisplayActionSheet("Action sheet without destruction", "Cancel", null);
            this.result.Text = $"Action sheet without destruction result: {result}";
        }

        async void ShowActionSheetWithRepeatedText()
        {
            var result = await DisplayActionSheet("Message", "Message", "Message", "Message", "Message");
            this.result.Text = $"Action sheet with repeated text result: {result}";
        }

        async void ShowActionSheetWhichTriggersAlert()
        {
            var result = await DisplayActionSheet("Trigger alert?", "No", null, "Yes");
            this.result.Text = $"Action sheet which triggers alert: {result}";
            if (result == "Yes") {
                await DisplayAlert("Alert", "Message", "Ok");
                this.result.Text = $"Alert result: Ok";
            }
        }
    }
}
