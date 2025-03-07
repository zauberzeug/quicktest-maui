using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace QuickTest
{
    public partial class User
    {
        void WireNavigation()
        {
            // NOTE: QuickTest currently only supports single window apps which do not replace the window after initial launch.
            var window = app.Windows[0];
            
            window.PropertyChanging += (s, args) => {
                if (args.PropertyName == nameof(Window.Page))
                    HandleMainPageChanging();
            };
            window.PropertyChanged += (s, args) => {
                if (args.PropertyName == nameof(Window.Page))
                    HandleMainPageChanged();
            };

            window.ModalPushing += HandleModalPushing;
            window.ModalPushed += HandleModalPushed;
            window.ModalPopping += HandleModalPopping;
            window.ModalPopped += HandleModalPopped;

            HandleMainPageChanged();
        }

        void HandleMainPageChanging() => HandlePageDisappearing(WindowPage);

        void HandleMainPageChanged() => HandlePageAppearing(WindowPage);

        void HandleFlyoutPagePropertyChanging(object sender, Microsoft.Maui.Controls.PropertyChangingEventArgs e)
        {
            var flyoutPage = sender as FlyoutPage;

            Page page = null;
            if (e.PropertyName == nameof(flyoutPage.Detail))
                page = flyoutPage.Detail;
            else if (e.PropertyName == nameof(flyoutPage.Flyout))
                page = flyoutPage.Flyout;

            if (page != null)
                HandlePageDisappearing(page);
        }

        void HandleFlyoutPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var flyoutPage = sender as FlyoutPage;

            Page page = null;
            if (e.PropertyName == nameof(flyoutPage.Detail))
                page = flyoutPage.Detail;
            else if (e.PropertyName == nameof(flyoutPage.Flyout))
                page = flyoutPage.Flyout;

            if (page != null)
                HandlePageAppearing(page);
        }

        // To be used with Page classes implementing IPageContainer.
        // Luckily, even though IPageContainer does not include the property changing event,
        // all classes implementing IPageContainer do send this event for CurrentPage.
        void HandleContainerPagePropertyChanging(object sender, Microsoft.Maui.Controls.PropertyChangingEventArgs e)
        {
            var pageContainer = sender as IPageContainer<Page>;
            if (e.PropertyName != nameof(pageContainer.CurrentPage))
                return;
            if (pageContainer.CurrentPage != null)
                HandlePageDisappearing(pageContainer.CurrentPage);
        }

        // To be used with Page classes implementing IPageContainer.
        // Luckily, even though IPageContainer does not include the property changed event,
        // all classes implementing IPageContainer do send this event for CurrentPage.
        void HandleContainerPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var pageContainer = sender as IPageContainer<Page>;
            if (e.PropertyName != nameof(pageContainer.CurrentPage))
                return;
            if (pageContainer.CurrentPage != null)
                HandlePageAppearing(pageContainer.CurrentPage);
        }

        void HandleModalPushing(object sender, ModalPushingEventArgs e) => HandlePageDisappearing(GetCurrentModalOrMainPage());

        void HandleModalPushed(object sender, ModalPushedEventArgs e) => HandlePageAppearing(e.Modal);

        void HandleModalPopping(object sender, ModalPoppingEventArgs e) => HandlePageDisappearing(e.Modal);

        void HandleModalPopped(object sender, ModalPoppedEventArgs e) => HandlePageAppearing(GetCurrentModalOrMainPage());

        Page GetCurrentModalOrMainPage() => WindowPage.Navigation.ModalStack.LastOrDefault() ?? WindowPage;

        void HandlePageAppearing(Page page)
        {
            EnablePlatform(page);
            SubscribeToPageEvents(page);
        }

        void HandlePageDisappearing(Page page)
        {
            UnsubscribeFromPageEvents(page);
        }

        void EnablePlatform(Page page)
        {
            page.IsPlatformEnabled = true;

            if (page is FlyoutPage flyoutPage) {
                EnablePlatform(flyoutPage.Flyout);
                EnablePlatform(flyoutPage.Detail);
            } else if (page is IPageContainer<Page> pageContainer) {
                EnablePlatform(pageContainer.CurrentPage);
            }
        }

        void SubscribeToPageEvents(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                flyoutPage.PropertyChanging += HandleFlyoutPagePropertyChanging;
                flyoutPage.PropertyChanged += HandleFlyoutPagePropertyChanged;
                SubscribeToPageEvents(flyoutPage.Flyout);
                SubscribeToPageEvents(flyoutPage.Detail);
            }

            if (page is IPageContainer<Page> pageContainer) {
                page.PropertyChanging += HandleContainerPagePropertyChanging;
                page.PropertyChanged += HandleContainerPagePropertyChanged;
                SubscribeToPageEvents(pageContainer.CurrentPage);
            }
        }

        void UnsubscribeFromPageEvents(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                flyoutPage.PropertyChanging -= HandleFlyoutPagePropertyChanging;
                flyoutPage.PropertyChanged -= HandleFlyoutPagePropertyChanged;
                UnsubscribeFromPageEvents(flyoutPage.Flyout);
                UnsubscribeFromPageEvents(flyoutPage.Detail);
            }

            if (page is IPageContainer<Page> pageContainer) {
                page.PropertyChanging -= HandleContainerPagePropertyChanging;
                page.PropertyChanged -= HandleContainerPagePropertyChanged;
                UnsubscribeFromPageEvents(pageContainer.CurrentPage);
            }
        }
    }
}
