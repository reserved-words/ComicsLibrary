using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
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

        public List<SeriesAction> Actions { get; set; }
    }

    
}
