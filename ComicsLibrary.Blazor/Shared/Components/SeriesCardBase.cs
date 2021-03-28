using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesCardBase : ComponentBase
    {
        [Parameter]
        public Series Series { get; set; }

        [Parameter]
        public List<SeriesAction> Actions { get; set; }
    }
}
