using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task OnAction(Series series, SeriesAction action)
        {
            var success = await action.ClickAction(series);

            if (success)
            {
                Items.Remove(series);
                StateHasChanged();
            }
        }
    }
}
