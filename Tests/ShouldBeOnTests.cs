using DemoApp;
using Microsoft.Maui.Controls;
using QuickTest;

namespace Tests;

[TestFixture]
public class ShouldBeOnTests : QuickTest<App>
{
    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();
        Launch(new App());
    }

    [Test]
    public void ShouldBeOn_PageWithAutomationId()
    {
        var page = new ContentPage { AutomationId = "TestPage" };
        App.MainPage = new NavigationPage(page);

        ShouldBeOn("TestPage");
    }

    [Test]
    public void ShouldBeOn_FlyoutPageWithAutomationId()
    {
        var flyoutPage = new FlyoutPage {
            Flyout = new ContentPage {
                AutomationId = "FlyoutPage",
                Title = "Title of the FlyoutPage",
            },
            Detail = new NavigationPage(new ContentPage { AutomationId = "DetailPage", })
        };
        App.MainPage = flyoutPage;

        flyoutPage.IsPresented = true;
        ShouldBeOn("FlyoutPage");
        flyoutPage.IsPresented = false;
        ShouldBeOn("DetailPage");
    }

    [Test]
    public void ShouldBeOn_ModalPageWithAutomationId()
    {
        var modalPage = new ContentPage { AutomationId = "ModalPage" };
        App.MainPage = new NavigationPage(new ContentPage());
        App.MainPage.Navigation.PushModalAsync(modalPage);

        ShouldBeOn("ModalPage");
    }

    [Test]
    public void ShouldBeOn_PageNotFound()
    {
        var page = new ContentPage { AutomationId = "TestPage" };
        App.MainPage = new NavigationPage(page);

        Assert.Throws<AssertionException>(() => ShouldBeOn("NonExistentPage"));
    }
}
