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

        [Inject]
        private ISearchService _searchService { get; set; }

        public List<BreadcrumbItem> Breadcrumbs => new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Home", href: "#"),
            new BreadcrumbItem("Search", null, disabled: true)
        };

        protected string[] errors = { };
        protected MudForm form;

        public bool Success { get; set; }
        public int SortOrder { get; set; }
        public int Source { get; set; }
        public string SearchText { get; set; }
        public int ItemsPerPage = 10;

        public bool ShowSearch { get; set; } = true;
        public bool ShowResults { get; set; } = false;
        public bool ShowNoResults { get; set; } = false;
        public int NumberOfResults { get; set; }
        public List<SearchResult> Results { get; set; } = new List<SearchResult>();
        public string ResultSearchText { get; set; }
        
        public bool Searching { get; set; }

        public async Task Search()
        {
            Results.Clear();
            NumberOfResults = 0;
            ShowNoResults = false;
            ShowResults = false;
            
            form.Validate();

            if (!errors.Any())
            {
                Searching = true;
                ShowSearch = false;

                var results = await _searchService.Search(Source, SearchText, SortOrder, ItemsPerPage, 1);

                NumberOfResults = results.TotalResults;
                Results = results.Results;

                ShowNoResults = NumberOfResults == 0;
                ShowResults = !ShowNoResults;
                Searching = false;
            }
        }

        public void Reset()
        {
            Results.Clear();
            NumberOfResults = 0;
            ShowResults = false;
            ShowNoResults = false;

            SearchText = null;
            SortOrder = 0;
            Source = 0;
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

            await FetchBooks(result);
        }

        public async Task FetchBooks(SearchResult result)
        {
            result.GettingBooks = true;

            var numberFetched = result.Books.Count();

            var nextResults = await _searchService.GetBooks(result.SourceId, result.SourceItemId, ItemsPerPage, numberFetched);

            result.TotalBooks = nextResults.TotalResults;

            result.Books.AddRange(nextResults.Results);

            result.MoreToFetch = result.Books.Count() < result.TotalBooks;

            result.GettingBooks = false;
        }
    }
}
