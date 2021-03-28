using ComicsLibrary.Common.Data;
using System;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Model
{
    public class SeriesAction
    {
        public string Caption { get; set; }
        public string Icon { get; set; }
        public Func<Series, Task> ClickAction { get; set; }
        public Shelf MoveToShelf { get; set; }
    }
}
