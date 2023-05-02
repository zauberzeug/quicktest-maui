using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace QuickTest
{
    public interface CrawlingStrategy
    {
        List<CellGroup> GetCellGroups(ListView listView);
    }
}
