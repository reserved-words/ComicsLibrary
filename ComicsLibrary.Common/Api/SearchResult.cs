using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common.Api
{
    public class SearchResult
    {
        public int SourceId { get; set; }
        public int SourceItemId { get; set; }
        public string Title { get; set; }
        public int LibraryId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
