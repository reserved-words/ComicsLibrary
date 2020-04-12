using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common
{
    public class SeriesUpdate
    {
        public SeriesUpdate()
        {
            Books = new List<BookUpdate>();
        }

        public string Title { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public virtual List<BookUpdate> Books { get; set; }
    }
}
