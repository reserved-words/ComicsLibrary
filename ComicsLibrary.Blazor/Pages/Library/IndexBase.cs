using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }

        [Inject]
        public ISeriesRepository Repository { get; set; }

        [Parameter]
        public string ShelfId { get; set; }

        public Shelf Shelf { get; set; }

        public string ShelfName { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        public List<Series> Items { get; set; } = new List<Series>();

        protected override async Task OnParametersSetAsync()
        {
            Actions = new List<SeriesAction>();
            Items = new List<Series>();

            Shelf = Enum.Parse<Shelf>(ShelfId);

            ShelfName = Shelf switch
            {
                Shelf.ToReadNext => "To Read Next",
                Shelf.PutAside => "Put Aside",
                _ => Shelf.ToString()
            };

            Actions = GetActions();

            var items = await Repository.GetShelf(Shelf, false);

            Items = items.Select(b => new Series(b)).ToList();
        }

        private List<SeriesAction> GetActions()
        {
            var Actions = new List<SeriesAction>();

            if (Shelf == Shelf.Archived)
            {
                Actions.Add(new SeriesAction { Caption = "Unarchive", Icon = Icons.Material.Filled.Unarchive, ClickAction = Unarchive });
                Actions.Add(new SeriesAction { Caption = "Delete", Icon = Icons.Material.Filled.DeleteForever, ClickAction = Delete });
            }
            else
            {
                if (Shelf == Shelf.Reading)
                {
                    Actions.Add(new SeriesAction { Caption = "Put Aside", Icon = Icons.Material.Filled.Pause, ClickAction = PutAside });
                }
                else
                {
                    Actions.Add(new SeriesAction { Caption = "Read Now", Icon = Icons.Material.Filled.PlayArrow, ClickAction = ReadNow });
                }

                if (Shelf == Shelf.ToReadNext)
                {
                    Actions.Add(new SeriesAction { Caption = "Remove from Read Next", Icon = Icons.Material.Filled.RemoveFromQueue, ClickAction = RemoveFromReadNext });
                }

                if (Shelf != Shelf.Reading && Shelf != Shelf.ToReadNext)
                {
                    Actions.Add(new SeriesAction { Caption = "Read Next", Icon = Icons.Material.Filled.PlaylistAdd, ClickAction = AddToReadNext });
                }

                Actions.Add(new SeriesAction { Caption = "Archive", Icon = Icons.Material.Filled.Archive, ClickAction = Archive });
            }

            return Actions;
        }

        private async Task MoveToShelf(Series series, Shelf shelf)
        {
            await DialogService.ShowMessageBox("Series Action", $"Move series {series.Title} to shelf {shelf}");
        }

        protected async Task Archive(Series series)
        {
            await MoveToShelf(series, Shelf.Archived);
        }

        protected async Task Unarchive(Series series)
        {
            //MoveToShelf(Series, Series.Progress == 0
            //        ? Shelf.Unread
            //        : Series.Progress == 100
            //        ? Shelf.Finished
            //        : Shelf.PutAside);
        }

        protected async Task AddToReadNext(Series series)
        {
            //MoveToShelf(Series, Shelf.ToReadNext);
        }

        protected async Task RemoveFromReadNext(Series series)
        {
            //MoveToShelf(Series, Shelf.Unread);
        }

        protected async Task ReadNow(Series series)
        {
            //MoveToShelf(Series, Shelf.Reading);
        }

        protected async Task PutAside(Series series)
        {
            //MoveToShelf(Series, Shelf.PutAside);
        }

        protected async Task Delete(Series series)
        {

        }
    }
}
