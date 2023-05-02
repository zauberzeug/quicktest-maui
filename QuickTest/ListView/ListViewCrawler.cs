using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace QuickTest
{
    public static class ListViewCrawler
    {
        static CrawlingStrategy retainStrategy = new RetainElementCrawlingStrategy();
        static CrawlingStrategy recycleStrategy = new RecycleElementCrawlingStrategy();

        public static List<CellGroup> GetCellGroups(ListView listView)
        {
            return GetCrawlingStrategy(listView.CachingStrategy).GetCellGroups(listView);
        }

        static CrawlingStrategy GetCrawlingStrategy(ListViewCachingStrategy cachingStrategy)
        {
            if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
                return recycleStrategy;
            else
                return retainStrategy;
        }
    }
}
