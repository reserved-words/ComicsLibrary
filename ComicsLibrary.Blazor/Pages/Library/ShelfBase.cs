using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class ShelfBase : ComponentBase
    {
        public IEnumerable<Series> Items { get; set; }
    }
}
