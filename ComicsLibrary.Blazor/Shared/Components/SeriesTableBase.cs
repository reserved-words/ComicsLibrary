using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesTableBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<Series> Series { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public Func<Series, bool> Filter { get; set; }

        public Series SelectedItem { get; set; }
    }
}
