using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Search
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private IDialogService _dialogService { get; set; }

        public List<BreadcrumbItem> Breadcrumbs => new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Home", href: "#"),
            new BreadcrumbItem("Search", null, disabled: true)
        };

        protected string[] errors = { };
        protected MudForm form;

        public bool Success { get; set; }
        public string SortOrder { get; set; }
        public string Source { get; set; }
        public string SearchText { get; set; }
        public int ItemsPerPage = 10;

        public bool ShowSearch { get; set; } = true;
        public bool ShowResults { get; set; } = false;
        public bool ShowNoResults { get; set; } = false;
        public int NumberOfResults { get; set; }
        public List<SearchResult> Results { get; set; } = new List<SearchResult>();
        public string ResultSearchText { get; set; }
        public SearchResult selectedItem;

        public void Search()
        {
            Results.Clear();
            ShowNoResults = false;
            ShowResults = false;
            NumberOfResults = 0;

            form.Validate();

            if (!errors.Any())
            {
                ShowSearch = false;
                var random = new Random(DateTime.Now.Second);
                NumberOfResults = random.Next(0, 10);

                for (var i = 0; i < NumberOfResults; i++)
                {
                    var isInLibrary = random.Next(0, 10) > 7;
                    var id = isInLibrary ? (100 * i + 1) : (int?)null;

                    Results.Add(new SearchResult
                    {
                        Id = id,
                        SourceId = 1,
                        SourceItemId = i + 1,
                        Title = $"Series {i + 1}",
                        Publisher = "Marvel Comics"
                    });
                }

                ShowNoResults = NumberOfResults == 0;
                ShowResults = !ShowNoResults;
            }
        }

        public void Reset()
        {
            Results.Clear();
            NumberOfResults = 0;
            SearchText = null;
            SortOrder = null;
            Source = null;
            ShowResults = false;
            ShowNoResults = false;
            ShowSearch = true;
            form.Reset();
        }

        public void GoToSeries(int id)
        {
            _dialogService.ShowMessageBox($"Go to Series", $"Go to series ID {id}");
        }

        public async Task AddSeries(SearchResult result)
        {
            result.Adding = true;
            await Task.Delay(3000);
            result.Id = new Random(DateTime.Now.Second).Next(1, 1000);
            result.Adding = false;
        }

        public async Task ShowBooks(SearchResult result)
        {
            result.ShowBooks = !result.ShowBooks;

            if (result.Books.Any())
            {
                return;
            }

            result.GettingBooks = true;
            await Task.Delay(3000);
            result.TotalBooks = new Random(DateTime.Now.Second).Next(1, 50);
            await FetchBooks(result);

            result.GettingBooks = false;
        }

        public async Task FetchBooks(SearchResult result)
        {
            var remainingToFetch = result.TotalBooks - result.Books.Count();

            var fetchNumber = Math.Min(remainingToFetch, ItemsPerPage);

            var firstId = result.Books.Count() + 1;

            for (var i = 0; i < fetchNumber; i++)
            {
                result.Books.Add(new SearchResultBook { Id = firstId + i });
            }
        }
    }
}
