﻿using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class NestedTabbedPageTests : QuickTest<App>
    {
        string expectedLog;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("NestedTabbedPage");

            expectedLog = "A(FlyoutPage) A(NavigationPage) A(Navigation) D(Navigation) D(NavigationPage) A(NavigationPage) A(NestedTabbedPage) A(Outer Tab A) A(Tab A) ";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
        }

        [Test]
        public void CanSeeAndSwitchToAllTabs()
        {
            ShouldSee("NestedTabbedPage", "Outer Tab A", "Outer Tab B", "Tab A", "Tab B", "This is content on tab A");
            ShouldSee("Outer Tab A", 1);
            ShouldNotSee("This is content on tab B", "This is content on outer tab B", "ToolbarItem", "OuterToolbarItem");

            Tap("Tab B");

            ShouldSee("This is content on tab B", "ToolbarItem");
            ShouldNotSee("This is content on tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Tab B) "));

            Tap("Tab A");

            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B", "ToolbarItem");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab B) A(Tab A) "));

            Tap("Outer Tab B");
            ShouldSee("This is content on outer tab B", "OuterToolbarItem");
            ShouldNotSee("This is content on tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) D(Outer Tab A) A(Outer Tab B) "));

            Tap("Outer Tab A");
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on outer tab B", "OuterToolbarItem");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Outer Tab B) A(Outer Tab A) A(Tab A) "));
        }
    }
}
