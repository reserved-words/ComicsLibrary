using ComicsLibrary.Common.Models;
using System.Collections.Generic;

namespace ComicsLibrary.Common.ViewModels
{
    public class LibraryViewModel
    {
        public List<Series> ToRead { get; set; }
        public List<Series> InProgress { get; set; }
        public List<Series> Finished { get; set; }
    }
}