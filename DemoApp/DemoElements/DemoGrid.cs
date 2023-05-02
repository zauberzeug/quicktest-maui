using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DemoApp
{
    public class DemoGrid : Grid
    {
        public DemoGrid()
        {
            RowDefinitions = new RowDefinitionCollection {
                new RowDefinition(),
                new RowDefinition(),
            };
            ColumnDefinitions = new ColumnDefinitionCollection {
                new ColumnDefinition(),
                new ColumnDefinition(),
            };
            this.Add(new DemoLabel("Cell A").WithGestureRecognizer(), 0, 0);
            this.Add(new DemoLabel("Cell B").WithGestureRecognizer(), 0, 1);
            this.Add(new DemoLabel("Cell C").WithGestureRecognizer(), 1, 0);
            this.Add(new DemoLabel("Cell D").WithGestureRecognizer(), 1, 1);
        }
    }
}
