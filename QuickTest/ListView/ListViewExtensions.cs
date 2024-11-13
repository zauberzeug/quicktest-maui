using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using ListViewItemsList = Microsoft.Maui.Controls.Internals.TemplatedItemsList<Microsoft.Maui.Controls.ItemsView<Microsoft.Maui.Controls.Cell>, Microsoft.Maui.Controls.Cell>;

namespace QuickTest;

public static class ListViewExtensions
{
    public static IEnumerable<ListViewItemsList> GetTemplatedItemsOfGroups(this ListView listView)
    {
        if (!listView.IsGroupingEnabled)
            throw new ArgumentException("This method can only be used for grouped list views!");
        
#pragma warning disable CA2021 // Don't call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types
        // As of MAUI 8.0.92:
        // - MAUI ListView uses the TemplatedItemsList type to store its items.
        // - TemplatedItemsList can be in "non-grouped" mode, and then contains a collection of cells.
        // - TemplatedItemsList can be in "grouped" mode, and then contains a collection of "non-grouped" TemplatedItemsList instances (one for each group).
        // - From the compiler's point of view, TemplatedItemsList is always an IEnumerable<Cell>.
        // - The IEnumerable<Cell>.GetEnumerator() implementation always returns cells and must only be used for "non-grouped" item lists.
        // - The IEnumerable.GetEnumerator() implementation behaves differently in "non-grouped" and "grouped" mode:
        //   - "non-grouped": Returns enumerator for cells
        //   - "grouped": Return enumerator for the sub item lists.
        //  - Unfortunately, there is no public API to get the sub item lists out of the parent item list.
        //  - Fortunately, we can use the general enumerator to access sub item lists, which is exactly what the cast
        //    statement below does.
        //  - Because the TemplatedItemsList implementation tells the compiler it is an IEnumerable<Cell>, the
        //    cast produces warning CA2021. But the cast works as long as the MAUI implementation does not change,
        //    so we ignore it for now.
        //  - Should the MAUI implementation change, an InvalidCastException should point us in the right direction.
        //  - Should the cast below no longer work, we should be able to access the sub item views via reflection.
        return listView.TemplatedItems.Cast<ListViewItemsList>();
#pragma warning restore CA2021
    }
}
