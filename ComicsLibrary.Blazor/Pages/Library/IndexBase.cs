using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ComponentBase
    {
        [Inject] 
        private IMessenger _messenger { get; set; }

        [Inject]
        private ISeriesRepository _repository { get; set; }

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

            Items = await _repository.GetShelf(Shelf, false);
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

        private async Task<bool> MoveToShelf(Series series, Shelf shelf)
        {
            var success = await _repository.UpdateShelf(series, shelf);

            if (success)
            {
                _messenger.DisplaySuccessAlert($"{series.Title} moved to shelf {shelf}");
            }
            else
            {
                _messenger.DisplayErrorAlert($"An error was encountered moving {series.Title} to shelf {shelf}");
            }

            return success;
        }

        protected async Task<bool> Archive(Series series)
        {
            return await MoveToShelf(series, Shelf.Archived);
        }

        protected async Task<bool> Unarchive(Series series)
        {
            return await MoveToShelf(series, series.Progress == 0
                    ? Shelf.Unread
                    : series.Progress == 100
                    ? Shelf.Finished
                    : Shelf.PutAside);
        }

        protected async Task<bool> AddToReadNext(Series series)
        {
            return await MoveToShelf(series, Shelf.ToReadNext);
        }

        protected async Task<bool> RemoveFromReadNext(Series series)
        {
            return await MoveToShelf(series, Shelf.Unread);
        }

        protected async Task<bool> ReadNow(Series series)
        {
            return await MoveToShelf(series, Shelf.Reading);
        }

        protected async Task<bool> PutAside(Series series)
        {
            return await MoveToShelf(series, Shelf.PutAside);
        }

        protected async Task<bool> Delete(Series series)
        {
            _messenger.DisplayErrorAlert("Deletion not currently available");
            return false;
        }
    }
}
