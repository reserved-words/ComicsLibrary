using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesDisplayBase : ComponentBase
    {
        [Parameter]
        public List<Series> Items { get; set; }

        [Parameter]
        public List<SeriesAction> Actions { get; set; }

        [Parameter]
        public bool Visible { get; set; }
    }
}
