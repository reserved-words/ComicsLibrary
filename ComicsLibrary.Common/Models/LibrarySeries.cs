namespace ComicsLibrary.Common
{
    public class LibrarySeries
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int UnreadBooks { get; set; }
        public int TotalBooks { get; set; }
        public bool Archived { get; set; }
        public int Progress { get; set; }

        public SeriesStatus Status => GetStatus();

        private SeriesStatus GetStatus()
        {
            if (Archived)
                return SeriesStatus.Archived;

            switch (Progress)
            {
                case 0:
                    return SeriesStatus.New;
                case 100:
                    return SeriesStatus.Finished;
                default:
                    return SeriesStatus.Reading;
            }
        }
    }
}
