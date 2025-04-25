using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp
{
    public class ElementDemoPage : ContentPage
    {
        public ElementDemoPage()
        {
            Title = "Element demo";
            PageExtensions.AddPageLog(this);

            var searchbar = new SearchBar {
                AutomationId = "searchbar_automation_id",
                BackgroundColor = Colors.White,
                HeightRequest = 48, // HACK: https://bugzilla.xamarin.com/show_bug.cgi?id=43975
            };

            searchbar.TextChanged += delegate {
                Application.Current.Windows[0].Page.
                           DisplayAlert("SearchBar Content",
                                        searchbar.Text != null ? $"<{searchbar.Text}>" : "null",
                                        "Ok");
            };

            Content = new ScrollView {
                Content = new StackLayout {
                    AutomationId = "page-stack",
                    Children = {
                        searchbar,
                        new DemoButton("Button"),
                        new DemoButton("Button with icon").WithFontImageSource("glyph"),
                        new DemoRadioButton("RadioButton") { GroupName = "group1" },
                        new DemoRadioButton("RadioButton2") { GroupName = "group1" },
                        new DemoRadioButton("RadioButton3") { GroupName = "group2" },
                        new DemoRadioButton("RadioButton4") { GroupName = "group2" },
                        new DemoLabel("Label").WithGestureRecognizer(),
                        CreateFormattedLabel(),
                        new DemoStack(),
                        new DemoGrid(),
                        new ContentView{Content = new DemoLabel("label within ContentView")},
                        new DemoEntry("entry_automation_id", "Placeholder"),
                        new DemoEditor("editor_automation_id", "editor content"),
                        new DemoLabel("Invisible Label").Invisible(),
                        new DemoSlider("slider_automation_id", 0, 120, 42),
                        new DemoPicker("picker_automation_id", "Pick an item", new List<PickerObject>{new PickerObject("Item A"), new PickerObject("Item B"), new PickerObject("Item C")}),
                        new DemoCountdown(),
                        new DemoImage("logo.png"),

                    },
                },
            };

            ToolbarItems.Add(new DemoToolbarItem());
        }

        DemoLabel CreateFormattedLabel()
        {

            return new DemoLabel("Label") {
                FormattedText = new FormattedString() {
                    Spans = {
                        new Span {
                            Text = "first line\nsecond line",
                        }
                    }
                }
            };
        }
    }

    public class PickerObject
    {
        public string Name;

        public PickerObject(string name) => Name = name;

        public override string ToString() => Name;
    }
}
