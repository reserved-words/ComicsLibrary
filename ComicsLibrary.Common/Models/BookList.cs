namespace ComicsLibrary.Common
{
    public class BookList
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public Comic[] Books { get; set; }
        public int TotalBooks { get; set; }
        public bool Home { get; set; }
    }
}
