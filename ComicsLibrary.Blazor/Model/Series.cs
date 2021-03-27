using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Model
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Number { get; set; }
        public string Publisher { get; set; }
        public string PublisherIcon { get; set; }
        public string Years { get; set; }
        public Color Color { get; set; }
        public string ImageUrl { get; set; }
        public int Progress { get; set; }
    }
}
