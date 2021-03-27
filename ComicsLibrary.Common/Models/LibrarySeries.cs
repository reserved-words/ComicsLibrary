using ComicsLibrary.Common.Data;

namespace ComicsLibrary.Common
{
    public class LibrarySeries
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool Archived { get; set; }
        public int Progress { get; set; }

        public string Publisher { get; set; }
        public string PublisherIcon { get; set; }
        public string Color { get; set; }
        public Shelf Shelf { get; set; }
    }
}
