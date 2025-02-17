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
    public void ShouldBeOn_Examples()
    {
        var page = new ShouldBeOnTestPage {
            Title = "some_title",
            AutomationId = "some_automation_id",
        };
        App.MainPage = new NavigationPage(page);

        ShouldBeOn(title: "some_title");
        ShouldBeOn(automationId: "some_automation_id");
        ShouldBeOn(title: "some_title", automationId: "some_automation_id");
        ShouldBeOn(type: typeof(ShouldBeOnTestPage));
        ShouldBeOn(predicate: (p) => p.Title == "some_title");
    }

    [Test]
    public void ShouldBeOn_PageNotFound()
    {
        var page = new ContentPage { AutomationId = "TestPage" };
        App.MainPage = new NavigationPage(page);

        Assert.Throws<AssertionException>(() => ShouldBeOn("NonExistentPage"));
    }
}

class ShouldBeOnTestPage : ContentPage
{
    public ShouldBeOnTestPage()
    {
    }
}
