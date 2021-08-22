using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class ReadingBookCardBase : ComponentBase
    {
        [Inject]
        private INavigator _nagivator { get; set; }

        [Inject]
        private IActionsService _actionsService { get; set; }

        [Inject]
        private Services.ISeriesRepository _repository { get; set; }

        [Inject]
        private IMessenger _messenger { get; set; }

        [Parameter]
        public NextComicInSeries Book { get; set; }

        [Parameter]
        public Func<NextComicInSeries, Task<bool>> SkipNext { get; set; }

        [Parameter]
        public Func<NextComicInSeries, Task<bool>> SkipPrevious { get; set; }

        public bool PreventSkipPrevious { get; set; }

        public bool PreventSkipNext { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        protected override void OnParametersSet()
        {
            PreventSkipPrevious = Book.Progress == 0;
            PreventSkipNext = Book.UnreadBooks == 1;

            Actions = _actionsService.GetSeriesActions(Common.Data.Shelf.Reading, true);

            StateHasChanged();
        }

        protected async Task<bool> SkipNext1()
        {
            return await SkipNext(Book);
        }

        protected async Task<bool> SkipPrevious1()
        {
            return await SkipPrevious(Book);
        }

        public async Task OnAction(SeriesAction action)
        {
            var series = await _repository.GetSeries(Book.SeriesId);

            var success = await action.ClickAction(series.Series);

            // Might need to update some stuff

            if (success)
            {
                _messenger.DisplayErrorAlert($"SUCCESSFUL ACTION: {action.Caption} {Book.SeriesId}");
            }
            else
            {
                _messenger.DisplayErrorAlert($"FAILED ACTION: {action.Caption} {Book.SeriesId}");
            }
        }
    }
}
