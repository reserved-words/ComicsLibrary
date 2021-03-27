using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class ToReadBase : ShelfBase
    {
        public override Shelf Shelf => Shelf.ToReadNext;
    }
}
