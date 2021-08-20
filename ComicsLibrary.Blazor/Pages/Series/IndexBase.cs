using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Series
{
    public class IndexBase : ComponentBase
    {
        [Inject] 
        private IMessenger _messenger { get; set; }

        [Inject]
        private Services.ISeriesRepository _repository { get; set; }

        [Parameter]
        public string SeriesId { get; set; }

        public SeriesDetail Item { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        protected override async Task OnParametersSetAsync()
        {
            Item = null;

            Item = await _repository.GetSeries(int.Parse(SeriesId));
        }

        public async Task OnAction(Model.Series series, SeriesAction action)
        {
            //var success = await action.ClickAction(series);

            //if (success)
            //{
            //    Items.Remove(series);
            //    StateHasChanged();
            //}

            //TO DO
        }
    }
}
