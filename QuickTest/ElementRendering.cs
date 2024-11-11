using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace QuickTest
{
    public static class ElementRendering
    {
        public static string Render(this Element element)
        {
            if (!(element as VisualElement)?.IsVisible ?? false)
                return "";

            var result = "· ";// + $"<{element.GetType().Name}> ";

            result += GetTitle(element as ContentPage);

            if (element is Page) {
                var tabbedPage = element.FindParent<TabbedPage>();
                var tabResult = "";
                while (tabbedPage != null) {
                    tabResult = "\n|" + string.Join("|", tabbedPage.Children.Select(p => tabbedPage.CurrentPage == p ? $"> {GetTabTitle(p)} <" : $" {GetTabTitle(p)} ")) + "|" + tabResult;
                    tabbedPage = tabbedPage.FindParent<TabbedPage>();
                }
                result += tabResult;
            }

            result += (element as ContentPage)?.Content?.Render();

            result += RenderContent(element);

            result = "\n" + result.Replace("\n", "\n  ");

            return result;
        }

        static string RenderContent(Element element)
        {
            var result = "";

            var automationId = (element as VisualElement)?.AutomationId;
            if (automationId != null)
                result += $"({automationId}) ";

            result += (element as ContentView)?.Content.Render();
            result += (element as ScrollView)?.Content.Render();
            result += string.Join("", (element as Layout)?.Children.Select(c => (c as Element)?.Render() ?? "") ?? new[] { "" });
            result += (element as ListView)?.Render();
            result += (element as Border)?.Content.Render();
            result += (element as Label)?.FormattedText?.ToString() ?? (element as Label)?.Text;
            result += (element as Button)?.Render();
            result += (element as Entry)?.Text;
            result += (element as Editor)?.Text;
            result += (element as SearchBar)?.Text;
            result += (element as Image)?.Source?.AutomationId;

            if (element is Picker) {
                var picker = element as Picker;
                if (picker.SelectedIndex >= 0)
                    result += picker.SelectedItem.ToString();
                else
                    result += picker.Title;
            }

            if (element is Slider)
                result += "--o---- " + (element as Slider).Value;

            result += (element as TextCell)?.Text;
            result += (element as ViewCell)?.View?.Render();

            return result;
        }

        static string GetTitle(ContentPage page)
        {
            var navigation = page?.Navigation;
            if (navigation == null)
                return ""; // without navigation the user can't see the title

            string result = "";
            var titleView = NavigationPage.GetTitleView(page);
            if (titleView != null)
                result += $"*{RenderContent(titleView).Replace("\n", "").Replace("· ", " ").Replace("  ", "")} *";
            else
                result += navigation.NavigationStack.LastOrDefault()?.Title;

            result += " " + string.Join(" ", page.ToolbarItems.Select(t => $"[{t.Text}]"));

            return result;
        }

        static string GetTabTitle(Page page)
        {
            if (!string.IsNullOrWhiteSpace(page.Title))
                return page.Title;
            else if (page.IconImageSource is FileImageSource fileImageSource)
                return fileImageSource.File;
            else
                return page.AutomationId;
        }

        public static string Render(this ListView listView)
        {
            var result = "";
            if (listView.ItemsSource == null)
                return result;

            result += Render(listView.Header);

            var groups = ListViewCrawler.GetCellGroups(listView);
            foreach (var group in groups)
                result += Render(group);

            result.TrimEnd('\n');
            result += Render(listView.Footer);

            return result;
        }

        static string Render(this Button button)
        {
            var texts = new List<string>();
            if (button.ImageSource is FontImageSource fontImageSource && !string.IsNullOrEmpty(fontImageSource.Glyph))
                texts.Add(fontImageSource.Glyph);
            if (!string.IsNullOrEmpty(button.Text))
                texts.Add(button.Text);
            return string.Join("|", texts);
        }

        static string Render(CellGroup cellGroup)
        {
            string result = "";

            if (cellGroup.Header != null)
                result += Render(cellGroup.Header).TrimStart('\n') + "\n";

            foreach (var cell in cellGroup.Content)
                result += $"- {(cell as TextCell)?.Text + (cell as ViewCell)?.View.Render().Trim()}\n";

            return result;
        }

        static string Render(object stringBindingOrView)
        {
            if (stringBindingOrView is string)
                return stringBindingOrView.ToString() + "\n";
            if (stringBindingOrView is View)
                return (stringBindingOrView as View).Render().TrimStart('\n') + "\n";

            return stringBindingOrView?.ToString() ?? "";
        }
    }
}
