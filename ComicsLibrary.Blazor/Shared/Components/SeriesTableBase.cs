using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesTableBase : SeriesDisplayBase
    {
        [Parameter]
        public Func<Series, bool> Filter { get; set; }

        public Series SelectedItem { get; set; }
    }
}
