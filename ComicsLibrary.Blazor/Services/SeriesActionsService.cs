using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeriesModel = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Services
{
    public class SeriesActionsService : ISeriesActionsService
    {
        private readonly INavigator _navigator;
        private readonly IMessenger _messenger;
        private readonly ISeriesRepository _repository;

        public SeriesActionsService(ISeriesRepository repository, IMessenger messenger, INavigator navigator)
        {
            _repository = repository;
            _messenger = messenger;
            _navigator = navigator;
        }

        public List<SeriesAction> GetActions(Shelf? shelf, bool includeView)
        {
            var actions = new List<SeriesAction>();

            if (includeView)
            {
                actions.Add(new SeriesAction { Caption = "View", Icon = Icons.Material.Filled.Details, ClickAction = ViewSeries });
            }

            if (!shelf.HasValue)
                return actions; // todo

            if (shelf == Shelf.Archived)
            {
                actions.Add(new SeriesAction { Caption = "Unarchive", Icon = Icons.Material.Filled.Unarchive, ClickAction = Unarchive });
                actions.Add(new SeriesAction { Caption = "Delete", Icon = Icons.Material.Filled.DeleteForever, ClickAction = Delete });
            }
            else
            {
                if (shelf == Shelf.Reading)
                {
                    actions.Add(new SeriesAction { Caption = "Put Aside", Icon = Icons.Material.Filled.Pause, ClickAction = PutAside });
                }
                else
                {
                    actions.Add(new SeriesAction { Caption = "Read Now", Icon = Icons.Material.Filled.PlayArrow, ClickAction = ReadNow });
                }

                if (shelf == Shelf.ToReadNext)
                {
                    actions.Add(new SeriesAction { Caption = "Remove from Read Next", Icon = Icons.Material.Filled.RemoveFromQueue, ClickAction = RemoveFromReadNext });
                }

                if (shelf != Shelf.Reading && shelf != Shelf.ToReadNext)
                {
                    actions.Add(new SeriesAction { Caption = "Read Next", Icon = Icons.Material.Filled.PlaylistAdd, ClickAction = AddToReadNext });
                }

                actions.Add(new SeriesAction { Caption = "Archive", Icon = Icons.Material.Filled.Archive, ClickAction = Archive });
            }

            return actions;
        }

        private async Task<bool> MoveToShelf(SeriesModel series, Shelf shelf)
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

        protected async Task<bool> Archive(SeriesModel series)
        {
            return await MoveToShelf(series, Shelf.Archived);
        }

        protected async Task<bool> Unarchive(SeriesModel series)
        {
            return await MoveToShelf(series, series.Progress == 0
                    ? Shelf.Unread
                    : series.Progress == 100
                    ? Shelf.Finished
                    : Shelf.PutAside);
        }

        protected async Task<bool> ViewSeries(SeriesModel series)
        {
            _navigator.NavigateToSeries(series.Id);
            return true;
        }

        protected async Task<bool> AddToReadNext(SeriesModel series)
        {
            return await MoveToShelf(series, Shelf.ToReadNext);
        }

        protected async Task<bool> RemoveFromReadNext(SeriesModel series)
        {
            return await MoveToShelf(series, Shelf.Unread);
        }

        protected async Task<bool> ReadNow(SeriesModel series)
        {
            return await MoveToShelf(series, Shelf.Reading);
        }

        protected async Task<bool> PutAside(SeriesModel series)
        {
            return await MoveToShelf(series, Shelf.PutAside);
        }

        protected async Task<bool> Delete(SeriesModel series)
        {
            _messenger.DisplayErrorAlert("Deletion not currently available");
            return false;
        }
    }
}
