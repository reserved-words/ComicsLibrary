using ComicsLibrary.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.ViewModels
{
    public class SearchViewModel
    {
        public int Page { get; set; }
        [Display(Name = "Series title starts with:")]
        public string TitleStartsWith { get; set; }
        [Display(Name = "Order results by:")]
        public int? OrderBy { get; set; }
        public PagedListViewModel<Series> Results { get; set; }
    }
}
