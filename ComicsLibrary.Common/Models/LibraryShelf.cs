using ComicsLibrary.Common.Api;
using System.Collections.Generic;

namespace ComicsLibrary.Common.Models
{
    public class LibraryShelf
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public List<LibrarySeries> Series { get; set; }
    }
}
