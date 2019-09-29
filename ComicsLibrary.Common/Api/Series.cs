using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Api
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Type { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Abandoned { get; set; }
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public string YearsActive { get; set; }
        public int Progress { get; set; }
        public int TotalComics { get; set; }
        public int UnreadAvailableComics { get; set; }
        public int MarvelId { get; set; }
        public string Url { get; set; }

        public Comic[] Comics { get; set; }
    }
}
