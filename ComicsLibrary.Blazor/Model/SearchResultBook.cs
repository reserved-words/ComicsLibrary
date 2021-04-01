namespace ComicsLibrary.Blazor.Model
{
    public class SearchResultBook
    {
        public int Id { get; set; }
        public string Title => $"Issue #{Id}";
    }
}
