using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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

        [Inject]
        private Services.IActionsService _actionsService { get; set; }

        [Parameter]
        public string SeriesId { get; set; }

        public SeriesDetail Item { get; set; }

        public List<BreadcrumbItem> Breadcrumbs { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        protected override async Task OnParametersSetAsync()
        {
            Item = null;

            Item = await _repository.GetSeries(int.Parse(SeriesId));

            UpdateActionsAndBreadcrumbs();
        }

        private void UpdateActionsAndBreadcrumbs()
        {
            Actions = _actionsService.GetSeriesActions(Item.Series.Shelf, false);

            Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("Home", href: "#"),
                new BreadcrumbItem("Library", href: "/library"),
                new BreadcrumbItem(Item.Series.Shelf.GetName(), href: $"/library/{(int)Item.Series.Shelf}"),
                new BreadcrumbItem(Item.Series.Title, href: null, disabled: true)
            };
        }

        public async Task OnAction(SeriesAction action)
        {
            var success = await action.ClickAction(Item.Series);

            if (success)
            {
                UpdateActionsAndBreadcrumbs();

                // Might also have set all books to read / unread

                StateHasChanged();
            }
            else
            {
                _messenger.DisplayErrorAlert($"FAILED ACTION: {action.Caption} {Item.Series.Id}");
            }
        }
    }
}
