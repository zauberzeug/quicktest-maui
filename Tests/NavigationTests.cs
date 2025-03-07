using DemoApp;
using Microsoft.Maui.Controls;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class NavigationTests : QuickTest<App>
    {
        string expectedLog;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());

            expectedLog = "A(NavigationPage) A(Navigation) A(FlyoutPage) ";
            VerifyEnabledPagesAndLog(expectedLog);
        }

        [Test]
        public void TestNavigationStack()
        {
            Tap("PushAsync");
            ShouldSee("Navigation >");
            ShouldNotSee("Navigation", "Menu");

            Tap("PushAsync");
            ShouldSee("Navigation > >");

            Tap("PopAsync");
            ShouldSee("Navigation >");

            Tap("PopAsync");
            ShouldSee("Navigation");

        }

        [Test]
        public void TestModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^"); // label is visible
            ShouldNotSee("Navigation ^"); // title is not visible because we have no navigation page with title bar
            ShouldNotSee("Navigation", "Menu");

            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^ ^");

            Tap("PopModalAsync");
            ShouldSee("Title: Navigation ^");
        }

        [Test]
        public void TestNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");
            ShouldSee("Title: Navigation ^");

            Tap("PushAsync");
            ShouldSee("Navigation ^ >");

            Tap("PopAsync");
            ShouldSee("Navigation ^");

            Tap("PopModalAsync");
            ShouldSee("Navigation");
        }

        [Test]
        public void TestGoBackOnNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");

            Tap("PushAsync");
            ShouldSee("Navigation ^ >");

            Tap("PushAsync");
            ShouldSee("Navigation ^ > >");

            GoBack();
            ShouldSee("Navigation ^ >");

            GoBack();
            ShouldSee("Navigation ^");

            GoBack();
            ShouldSee("Navigation");
        }

        [Test]
        public void TestGoBackOnPageOnModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^");

            GoBack();
            ShouldSee("Navigation");
        }

        [Test]
        public void TestPopToRoot()
        {
            Tap("PushAsync");
            Tap("PushAsync");
            Tap("PushAsync");
            ShouldSee("Navigation > > >");

            Tap("PopToRootAsync");
            ShouldSee("Navigation");
        }

        [Test]
        public void TestPageAppearingOnAppStart()
        {
            VerifyEnabledPagesAndLog("A(NavigationPage) A(Navigation) A(FlyoutPage) ");
        }

        [Test]
        public void TestPageDisAppearingOnPushPop()
        {
            Tap("PushAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) A(Navigation >) ");

            GoBack();
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation >) A(Navigation) ");
        }

        [Test]
        public void TestPageDisAppearingOnModalPushPop()
        {
            Tap("PushModalAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) D(FlyoutPage) A(Navigation ^) ");

            Tap("PopModalAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation ^) A(NavigationPage) A(Navigation) A(FlyoutPage) ");
        }

        [Test]
        public void TestPageDisAppearingOnMenuChange()
        {
            OpenMenu("Elements");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) A(NavigationPage) A(Element demo) ");

            OpenMenu("Navigation");
            VerifyEnabledPagesAndLog(expectedLog += "D(Element demo) D(NavigationPage) A(NavigationPage) A(Navigation) ");

            Tap("PushAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) A(Navigation >) ", "normal navigation should still be possible after menu change");
        }

        [Test]
        public void TestPopToRootEvent()
        {
            Tap("PushAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) A(Navigation >) ");

            Tap("PopToRootAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation >) A(Navigation) ");
        }

        [Test]
        public void TestModalPopToRootEvent()
        {
            Tap("PushModalAsync NavigationPage");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) D(FlyoutPage) A(NavigationPage) A(Navigation ^) ");

            Tap("PushAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation ^) A(Navigation ^ >) ");

            Tap("PopToRootAsync");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation ^ >) A(Navigation ^) ");
        }

        [Test]
        public void ToggleMainPageBetweenFlyoutAndNavigation()
        {
            Tap("Toggle Flyout MainPage");
            ShouldSee("Navigation");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) D(FlyoutPage) A(NavigationPage) A(Navigation) ");

            Tap("PushAsync");
            ShouldSee("Navigation >");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) A(Navigation >) ");

            Tap("PopAsync");
            ShouldSee("Navigation");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation >) A(Navigation) ");

            Tap("Toggle Flyout MainPage");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) A(NavigationPage) A(Navigation) A(FlyoutPage) ");

            OpenMenu("Elements");
            ShouldSee("Element demo");
            VerifyEnabledPagesAndLog(expectedLog += "D(Navigation) D(NavigationPage) A(NavigationPage) A(Element demo) ");
        }

        [Test]
        public void CanDisplayAlertOnModalPageWithoutFurtherNavigation()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");

            Tap("Show Alert");
            ShouldSee("Alert title", "Alert message", "Ok");
            Tap("Ok");
        }

        void VerifyEnabledPagesAndLog(string log, string message = null)
        {
            Assert.That(App.PageLog, Is.EqualTo(log), message);
            VerifyEnabledPages();
        }

        void VerifyEnabledPages() => VerifyEnabled(App.Windows[0].Page);

        void VerifyEnabled(Page page)
        {
            Assert.That(page.IsPlatformEnabled, Is.True, "Page must be platform enabled");
            if (page is FlyoutPage flyoutPage) {
                VerifyEnabled(flyoutPage.Flyout);
                VerifyEnabled(flyoutPage.Detail);
            } else if (page is IPageContainer<Page> pageContainer) {
                VerifyEnabled(pageContainer.CurrentPage);
            }
        }
    }
}
