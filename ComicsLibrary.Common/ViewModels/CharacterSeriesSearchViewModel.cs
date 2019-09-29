using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.ViewModels
{
    public class CharacterSeriesSearchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PagedListViewModel<Series> Results { get; set; }
    }
}
