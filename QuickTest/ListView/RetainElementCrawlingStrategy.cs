using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using ListViewItemsList = Microsoft.Maui.Controls.Internals.TemplatedItemsList<Microsoft.Maui.Controls.ItemsView<Microsoft.Maui.Controls.Cell>, Microsoft.Maui.Controls.Cell>;

namespace QuickTest
{
    public class RetainElementCrawlingStrategy : CrawlingStrategy
    {
        public List<CellGroup> GetCellGroups(ListView listView)
        {
            if (listView.IsGroupingEnabled)
                return listView.GetTemplatedItemsOfGroups().Select(t => GetCellGroup(t)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView.TemplatedItems) };
        }

        static CellGroup GetCellGroup(ListViewItemsList templatedItems)
        {
            return new CellGroup() {
                Header = templatedItems.HeaderContent,
                Content = templatedItems.ToList(),
            };
        }
    }
}
