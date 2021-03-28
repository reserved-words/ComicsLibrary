using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Parameter]
        public string ShelfId { get; set; }

        public Shelf Shelf { get; set; }

        public string ShelfName { get; set; }

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        public List<Model.Series> Items { get; set; } = new List<Model.Series>();

        protected override async Task OnParametersSetAsync()
        {
            Actions.Clear();
            Items.Clear();

            Shelf = Enum.Parse<Shelf>(ShelfId);

            ShelfName = Shelf switch
            {
                Shelf.ToReadNext => "To Read Next",
                Shelf.PutAside => "Put Aside",
                _ => Shelf.ToString()
            };

            var shelf = (int)Shelf;
            var url = $"http://localhost:58281/Library/Shelf?shelf={shelf}";
            var series = await HttpClient.GetFromJsonAsync<LibrarySeries[]>(url);
            Items = series.Select(b => new Model.Series(b)).ToList();

            Actions = GetActions();
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

        protected void Archive(MouseEventArgs args)
        {
            //MoveToShelf(Series, Shelf.Archived);
        }

        protected void Unarchive(MouseEventArgs args)
        {
            //MoveToShelf(Series, Series.Progress == 0
            //        ? Shelf.Unread
            //        : Series.Progress == 100
            //        ? Shelf.Finished
            //        : Shelf.PutAside);
        }

        protected void AddToReadNext(MouseEventArgs args)
        {
            //MoveToShelf(Series, Shelf.ToReadNext);
        }

        protected void RemoveFromReadNext(MouseEventArgs args)
        {
            //MoveToShelf(Series, Shelf.Unread);
        }

        protected void ReadNow(MouseEventArgs args)
        {
            //MoveToShelf(Series, Shelf.Reading);
        }

        protected void PutAside(MouseEventArgs args)
        {
            //MoveToShelf(Series, Shelf.PutAside);
        }

        protected void Delete(MouseEventArgs args)
        {

        }
    }
}
