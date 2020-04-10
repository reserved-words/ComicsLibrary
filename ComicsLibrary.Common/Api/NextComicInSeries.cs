using System;

namespace ComicsLibrary.Common.Api
{
    public class NextComicInSeries
    {
        public int Id { get; set; }
        public string SeriesTitle { get; set; }
        public string IssueTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReadUrl { get; set; }
        public int SeriesId { get; set; }
        public string OnSaleDate { get; set; }
        public int UnreadIssues { get; set; }
        public string Creators { get; set; }
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }
}
