using System;
using System.Linq;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Tests
{
    public class ElementTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("Elements");
        }

        [Test]
        public void TestLabel()
        {
            Tap("Label");
            ShouldSee("Label tapped");
        }

        [Test]
        public void TestFormattedLabel()
        {
            ShouldSee("first line\nsecond line");
        }

        [Test]
        public void TestButton()
        {
            Tap("Button");
            ShouldSee("Button tapped");
        }

        [Test]
        public void TestRadioButton()
        {
            void ShouldBeChecked(string text, bool isChecked) => Assert.That((FindFirst(text) as RadioButton).IsChecked, Is.EqualTo(isChecked));

            Tap("RadioButton3");
            ShouldSee("RadioButton3 checked");
            Tap("Ok");
            ShouldBeChecked("RadioButton", false);
            ShouldBeChecked("RadioButton2", false);
            ShouldBeChecked("RadioButton3", true);
            ShouldBeChecked("RadioButton4", false);

            Tap("RadioButton");
            ShouldSee("RadioButton checked");
            Tap("Ok");
            ShouldBeChecked("RadioButton", true);
            ShouldBeChecked("RadioButton2", false);

            Tap("RadioButton2");
            ShouldSee("RadioButton2 checked");
            Tap("Ok");
            ShouldSee("RadioButton unchecked");
            Tap("Ok");
            ShouldBeChecked("RadioButton", false);
            ShouldBeChecked("RadioButton2", true);

            // Radio buttons in a different group are not affected
            ShouldBeChecked("RadioButton3", true);
            ShouldBeChecked("RadioButton4", false);
        }

        [Test]
        public void TestButton_FontImageSource()
        {
            ShouldSee("Button with icon");
            Tap("glyph");
            ShouldSee("Button with icon tapped");
        }

        [Test]
        public void TestNestedLabel()
        {
            Tap("label in tap-able layout");
            ShouldSee("StackLayout tapped");
        }

        [Test]
        public void TestGrid()
        {
            Tap("Cell D");
            ShouldSee("Cell D tapped");
        }

        [Test]
        public void TestLabelWithinContentView()
        {
            ShouldSee("label within ContentView");
        }

        [Test]
        public void TestInvisible()
        {
            ShouldNotSee("Invisible Label");
        }

        [Test]
        public void TestEntry()
        {
            ShouldSee("Placeholder", "entry_automation_id");

            Input("Placeholder", "Text1");
            ShouldSee("Text1");
            ShouldNotSee("Placeholder");

            Input("entry_automation_id", "Text2");
            ShouldSee("Text2");

            Input("Text2", "Text3");
            ShouldSee("Text3");
        }

        [Test]
        public void TestEntryCompletionAndFocus()
        {
            var entry = Find("entry_automation_id").First() as Entry;
            EventHandler onCompleted = (object sender, EventArgs args) => {
                (sender as Entry).Text += "<completed>";
            };

            EventHandler<FocusEventArgs> onUnfocused = (object sender, FocusEventArgs args) => {
                (sender as Entry).Text += "<unfocused>";
            };

            entry.Completed += onCompleted;
            entry.Unfocused += onUnfocused;

            Input("entry_automation_id", "text");
            ShouldSee("text<completed><unfocused>");

            entry.Completed -= onCompleted;
            entry.Unfocused -= onUnfocused;

            Input("entry_automation_id", "text");
            ShouldSee("text");
        }

        [Test]
        public void TestEntryMaxLength()
        {
            var entry = Find("entry_automation_id").First() as Entry;
            entry.MaxLength = 10;

            Input("entry_automation_id", "Text 1234567890");
            ShouldNotSee("Text 1234567890");
            ShouldSee("Text 12345");
        }

        [Test]
        public void TestEditor()
        {
            ShouldSee("editor_automation_id");

            Input("editor_automation_id", "Text1");
            ShouldSee("Text1");

            Input("Text1", "Text2");
            ShouldSee("Text2");
        }

        [Test]
        public void TestEditorMaxLength()
        {
            var editor = Find("editor_automation_id").First() as Editor;
            editor.MaxLength = 10;

            Input("editor_automation_id", "Text 1234567890");
            ShouldNotSee("Text 1234567890");
            ShouldSee("Text 12345");
        }

        [Test]
        public void TestToolbarItem()
        {
            Tap("ToolbarItem");
            ShouldSee("ToolbarItem tapped");
        }

        [Test]
        public void TestSearchbar()
        {
            Input("searchbar_automation_id", "");
            ShouldSee("<>");
            Tap("Ok");

            Input("searchbar_automation_id", "Search Text");
            ShouldSee("<Search Text>");
            Tap("Ok");

            Cancel("searchbar_automation_id");
            ShouldSee("null");
        }

        [Test]
        public void Slider()
        {
            var expectedLog = "A(NavigationPage) A(Navigation) A(FlyoutPage) D(Navigation) D(NavigationPage) A(NavigationPage) A(Element demo) ";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));

            Input("slider_automation_id", 10);
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "slider:10 "));

            Input("slider_automation_id", 130);
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "slider:120 "), "max value should be 120");
        }

        [Test]
        public void Picker()
        {
            ShouldSee("Pick an item");
            ShouldNotSee("Item B");

            Pick("picker_automation_id", "Item B");
            ShouldSee("Item B");

            var picker = FindFirst("picker_automation_id") as Picker;
            var pickerObject = picker.SelectedItem as PickerObject;
            Assert.That(pickerObject, Is.Not.Null);
            Assert.That(pickerObject.Name, Is.EqualTo("Item B"));

            Assert.Throws<InvalidOperationException>(() => Pick("picker_automation_id", "Item D"));
            Assert.Throws<InvalidOperationException>(() => Pick("slider_automation_id", "Item A"));
        }

        [Test]
        public void ImageFromRessource()
        {
            ShouldSee("logo.png");
        }
    }
}
