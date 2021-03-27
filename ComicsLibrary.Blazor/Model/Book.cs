using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string SeriesTitle { get; set; }
        public string Years { get; set; }
        public string IssueTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReadUrl { get; set; }
        public string Publisher { get; set; }
        public string PublisherIcon { get; set; }
        public Color Color { get; set; }
        public int Progress { get; set; }
    }
}
