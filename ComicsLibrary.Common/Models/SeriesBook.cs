namespace ComicsLibrary.Common.Models
{
    public class SeriesBook
    {
        public int Id { get; set; }
        public string ReadUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public bool Hidden { get; set; }
        public int BookTypeID { get; set; }
        public int? Number { get; set; }
    }
}