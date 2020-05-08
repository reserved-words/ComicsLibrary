using System.Collections.Generic;

namespace ComicsLibrary.Common
{
    public class SeriesWithBooks
    {
        public SeriesSummary Summary { get; set; }
        public List<SeriesBook> Books { get; set; }
        public List<SeriesBookType> BookTypes { get; set; }
    }
}