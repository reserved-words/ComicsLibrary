using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.ViewModels
{
    public class SeriesComicSearchViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public PagedListViewModel<Comic> Results { get; set; }
    }
}
