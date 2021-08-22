using ComicsLibrary.Common.Data;
using System;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Model
{
    public class BookAction
    {
        public string Caption { get; set; }
        public string Icon { get; set; }
        public Func<Common.Comic, Task<bool>> ClickAction { get; set; }
    }
}
