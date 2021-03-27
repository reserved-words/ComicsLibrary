using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class ShelfBase : ComponentBase
    {
        public List<Series> Items { get; set; }
    }
}
