using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesCardBase : ComponentBase
    {
        [Parameter]
        public Model.Series Series { get; set; }
    }
}
