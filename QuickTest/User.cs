using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
    public partial class User
    {
        readonly Application app;
        readonly List<Popup> popups = new List<Popup>();

        public User(Application app)
        {
            this.app = app;
            (app as IApplication).CreateWindow(null);

            app.Invoke("OnStart");

#pragma warning disable CS0618 // Type or member is obsolete (MAUI currently still uses MessagingCenter internally)
            MessagingCenter.Subscribe<Page, AlertArguments>(this, Page.AlertSignalName, (page, alert) => {
                popups.Add(new AlertPopup(alert));
            });

            MessagingCenter.Subscribe<Page, ActionSheetArguments>(this, Page.ActionSheetSignalName, (page, actionSheet) => {
                popups.Add(new ActionSheetPopup(actionSheet));
            });
#pragma warning restore CS0618 // Type or member is obsolete

            WireNavigation();
        }

        public void Cleanup()
        {
#pragma warning disable CS0618 // Type or member is obsolete (MAUI currently still uses MessagingCenter internally)
            MessagingCenter.Unsubscribe<Page, AlertArguments>(this, Page.AlertSignalName);
            MessagingCenter.Unsubscribe<Page, ActionSheetArguments>(this, Page.ActionSheetSignalName);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public NavigationPage CurrentNavigationPage {
            get {
                var modalNavigationPage = app.MainPage.Navigation.ModalStack.LastOrDefault() as NavigationPage;
                if (modalNavigationPage != null)
                    return modalNavigationPage;
                return (app.MainPage as NavigationPage)
                ?? (app.MainPage as FlyoutPage).Detail as NavigationPage;
            }
        }

        public Page CurrentPage {
            get {
                var outermostPage = app.MainPage.Navigation.ModalStack.LastOrDefault() ?? app.MainPage;
                var currentPage = FindInnermostPage(outermostPage);

                if (currentPage == null)
                    Assert.Fail("CurrentPage not found");

                return currentPage;
            }
        }

        public bool CanSee(string text)
        {
            if (popups.Any())
                return popups.Last().Contains(text);
            else
                return CurrentPage.Find(text).Any();
        }

        public bool CanSee(string text, int count)
        {
            if (popups.Any())
                return popups.Last().Count(text) == count;
            else
                return CurrentPage.Find(text).Count == count;
        }

        public bool SeesPopup() => popups.Any();

        public bool SeesAlert() => popups.Any() && popups.Last() is AlertPopup;

        public bool SeesActionSheet() => popups.Any() && popups.Last() is ActionSheetPopup;

        public List<Element> Find(string text)
        {
            return CurrentPage.Find(text).Select(i => i.Element).ToList();
        }

        public List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return CurrentPage.Find(predicate, containerPredicate).Select(i => i.Element).ToList();
        }

        public void Tap(string text, int? index = null)
        {
            if (popups.Any()) {
                Assert.That(index, Is.Null, "Tap indices are not supported on alerts");
                var popup = popups.Last();
                if (popup.Tap(text))
                    popups.Remove(popup);
                else
                    Assert.Fail($"Could not tap \"{text}\" on popup\n{popup}");
                return;
            }

            var elementInfos = CurrentPage.Find(text);
            Assert.That(elementInfos, Is.Not.Empty, $"Did not find \"{text}\" on current page");

            ElementInfo elementInfo;
            if (index == null) {
                Assert.That(elementInfos, Has.Count.LessThan(2), $"Found multiple \"{text}\" on current page");
                elementInfo = elementInfos.First();
            } else {
                Assert.That(elementInfos, Has.Count.GreaterThan(index), $"Did not find enough \"{text}\" on current page");
                elementInfo = elementInfos.Skip(index.Value).First();
            }

            if (elementInfo.Element is ToolbarItem)
                (elementInfo.Element as ToolbarItem).Command.Execute(null);
            else if (elementInfo.Element is Button)
                (elementInfo.Element as Button).Command.Execute(null);
            else if (elementInfo.InvokeTap != null)
                elementInfo.InvokeTap.Invoke();
            else
                throw new InvalidOperationException($"element with text '{text}' is not tappable");
        }

        public void Input(string automationId, string text)
        {
            var elements = FindElements(automationId);

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
            if (elements.First() is Entry) {
                var maxTextLength = Math.Min(text.Length, (elements.First() as Entry).MaxLength);
                (elements.First() as Entry).Text = text.Substring(0, maxTextLength);
                (elements.First() as Entry).SendCompleted();
            } else if (elements.First() is Editor) {
                var maxTextLength = Math.Min(text.Length, (elements.First() as Editor).MaxLength);
                (elements.First() as Editor).Text = text.Substring(0, maxTextLength);
                (elements.First() as Editor).SendCompleted();
            } else if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = text;
            else if (elements.First() is Slider)
                (elements.First() as Slider).Value = double.Parse(text);
            else
                throw new InvalidOperationException($"element '{automationId}' cannot be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        public void Input(string automationId, int value)
        {
            Input(automationId, value.ToString());
        }

        public void Pick(string automationId, string text)
        {
            var elements = FindElements(automationId);
            if (elements.First() is Picker picker) {
                var indexToSelect = picker.Items.IndexOf(text);
                if (indexToSelect == -1)
                    throw new InvalidOperationException($"picker does not contain item '{text}'");
                picker.SelectedIndex = indexToSelect;
            } else
                throw new InvalidOperationException($"element '{automationId}' is not a Picker");
        }

        public void Cancel(string automationId)
        {
            var elements = FindElements(automationId);

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = null;
            else
                throw new InvalidOperationException($"element '{automationId}' cannot be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        public void OpenMenu()
        {
            (app.MainPage as FlyoutPage).IsPresented = true;
        }

        public void CloseMenu()
        {
            (app.MainPage as FlyoutPage).IsPresented = false;
        }

        public void GoBack()
        {
            var modalPage = app.MainPage.Navigation.ModalStack.LastOrDefault();
            if (modalPage != null) {
                // Xamarin.Forms expects a synchronization context when popping a page from the modal stack via back button press
                try {
                    SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                    modalPage.SendBackButtonPressed();
                } finally {
                    SynchronizationContext.SetSynchronizationContext(null);
                }
            } else
                app.MainPage.SendBackButtonPressed();
        }

        public void Print()
        {
            Console.WriteLine(Render());
        }

        public string Render()
        {
            if (popups.Any())
                return popups.Last().Render();
            else
                return CurrentPage.Render().Trim();
        }

        List<VisualElement> FindElements(string automationId)
        {
            var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<VisualElement>().ToList();

            Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
            Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");

            return elements;
        }

        Page FindInnermostPage(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                if (flyoutPage.IsPresented)
                    return FindInnermostPage(flyoutPage.Flyout);
                else
                    return FindInnermostPage(flyoutPage.Detail);
            }

            if (page is IPageContainer<Page> pageContainer)
                if (pageContainer.CurrentPage != null)
                    return FindInnermostPage(pageContainer.CurrentPage);

            return page;
        }

        public bool ShouldBeOn(Predicate<Page> predicate)
        {
            var currentPage = CurrentPage;
            var pageMatchingPredicate = FindPage(currentPage, predicate);

            return pageMatchingPredicate != null;
        }

        private Page FindPage(Page page, Predicate<Page> predicate)
        {
            if (predicate(page))
                return page;

            if (page is FlyoutPage flyoutPage) {
                var flyoutResult = FindPage(flyoutPage.Flyout, predicate);
                if (flyoutResult != null)
                    return flyoutResult;

                var detailResult = FindPage(flyoutPage.Detail, predicate);
                if (detailResult != null)
                    return detailResult;
            }

            if (page is IPageContainer<Page> pageContainer) {
                var currentPage = pageContainer.CurrentPage;
                if (currentPage != null) {
                    var containerResult = FindPage(currentPage, predicate);
                    if (containerResult != null)
                        return containerResult;
                }
            }

            return null;
        }
    }
}
