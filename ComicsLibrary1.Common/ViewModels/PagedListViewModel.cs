using System.Collections.Generic;

namespace ComicsLibrary.Common.ViewModels
{
    public class PagedListViewModel<T>
    {
        public PagedListViewModel(List<T> results, int page, int resultsPerPage, int? total)
        {
            Results = results;
            Total = total ?? results.Count;
            Page = page;

            LastPage = (Total / resultsPerPage) + 1;
            PreviousPage = Page == 1 ? (int?)null : Page - 1;
            NextPage = Page == LastPage ? (int?)null : Page + 1;
        }

        public List<T> Results { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public int FirstPage => 1;
        public int LastPage { get; set; }
    }
}
