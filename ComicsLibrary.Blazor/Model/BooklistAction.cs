using ComicsLibrary.Common;
using System;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Model
{
    public class BooklistAction
    {
        public string Caption { get; set; }
        public string Icon { get; set; }
        public Func<BookList, Task<bool>> ClickAction { get; set; }
    }
}
