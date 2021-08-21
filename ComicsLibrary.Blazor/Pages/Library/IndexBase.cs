using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeriesModel = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private ISeriesRepository _repository { get; set; }

        [Inject]
        private IActionsService _actionsService { get; set; }

        [Parameter]
        public string ShelfId { get; set; }

        public Shelf Shelf { get; set; }

        public string ShelfName { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        public List<SeriesModel> Items { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Actions = new List<SeriesAction>();
            Items = null;

            Shelf = Enum.Parse<Shelf>(ShelfId);

            ShelfName = Shelf switch
            {
                Shelf.ToReadNext => "To Read Next",
                Shelf.PutAside => "Put Aside",
                _ => Shelf.ToString()
            };

            Actions = _actionsService.GetActions(Shelf, true);

            Items = await _repository.GetShelf(Shelf, false);
        }
    }
}
