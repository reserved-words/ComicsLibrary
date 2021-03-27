using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public abstract class ShelfBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<Model.Series> Items { get; set; } = new List<Model.Series>();

        public abstract Shelf Shelf { get; }

        protected override async Task OnInitializedAsync()
        {
            var shelf = (int)Shelf;
            var url = $"http://localhost:58281/Library/Shelf?shelf={shelf}";
            var series = await HttpClient.GetFromJsonAsync<LibrarySeries[]>(url);
            Items = series.Select(b => new Model.Series(b)).ToList();
        }
    }
}
