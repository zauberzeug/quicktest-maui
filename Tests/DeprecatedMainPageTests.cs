using System;
using Microsoft.Maui.Controls;
using QuickTest;
using DemoApp;

namespace Tests;

// QuickTest now uses Window instead of the deprecated MainPage.
// As of MAUI 9.0.30, using Application.MainPage still works without additional code.
public class DeprecatedMainPageApp : Application
{
    public string PageLog = "";
    
    public DeprecatedMainPageApp()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        MainPage = AddPageLog(new FlyoutPage {
            Flyout = new MenuPage(),
            Detail = AddPageLog(new NavigationPage(AddPageLog(new ContentPage()))),
        });
#pragma warning restore CS0618 // Type or member is obsolete
    }
    
    public Page AddPageLog(Page page) => page.AddPageLog((s) => PageLog += s);
}

public class DeprecatedMainPageTests : QuickTest<DeprecatedMainPageApp>
{
    string expectedLog;
    
    protected override void SetUp()
    {
        base.SetUp();
        Launch(new DeprecatedMainPageApp());
        
        expectedLog = "A(NavigationPage) A(ContentPage) A(FlyoutPage) ";
        VerifyEnabledPagesAndLog(expectedLog);
    }

    [Test]
    public void TestMainPageChange()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Application.Current.MainPage = App.AddPageLog(new NavigationPage(App.AddPageLog(new ContentPage())));
#pragma warning restore CS0618 // Type or member is obsolete
        VerifyEnabledPagesAndLog(expectedLog += "D(ContentPage) D(NavigationPage) D(FlyoutPage) A(NavigationPage) A(ContentPage) ");
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
