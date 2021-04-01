using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Blazor.Model
{
    public class SearchResult
    {
        public int? Id { get; set; }
        public int SourceId { get; set; }
        public int SourceItemId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public bool ShowBooks { get; set; }
        public bool Adding { get; set; }
        public bool GettingBooks { get; set; }
        public int TotalBooks { get; set; }
        public List<SearchResultBook> Books { get; set; } = new List<SearchResultBook>();
        public bool MoreToFetch => Books.Count() < TotalBooks;
    }
}
