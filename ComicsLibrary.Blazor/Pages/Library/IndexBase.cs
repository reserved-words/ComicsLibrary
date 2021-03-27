using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ShelfBase
    {
        public override Shelf Shelf => Shelf.Archived; // TO DO - FIX TO GET ALL
    }
}
