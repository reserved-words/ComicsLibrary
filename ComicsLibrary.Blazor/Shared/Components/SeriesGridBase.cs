using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesGridBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<Series> Series { get; set; }

        [Parameter]
        public bool Visible { get; set; }
    }
}
