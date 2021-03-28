using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesCardBase : ComponentBase
    {
        [Parameter]
        public Series Series { get; set; }

        [Parameter]
        public List<SeriesAction> Actions { get; set; }

        [Parameter]
        public Func<Series, SeriesAction, Task> OnAction { get; set; }
    }
}
