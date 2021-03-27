using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<NextComicInSeries> Books { get; set; } = new List<NextComicInSeries>();

        protected override async Task OnInitializedAsync()
        {
            var url = "http://localhost:58281/Library/GetAllNextUnread";

            var books = await HttpClient.GetFromJsonAsync<NextComicInSeries[]>(url);

            foreach (var book in books)
            {
                (var title, var years) = book.SplitSeriesTitle();
                book.SeriesTitle = title;
                book.Years = years;
            }

            Books = books.ToList();

            //var books = 

            //Books = new List<Book>
            //{
            //    new Book { 
            //        Id = 1, 
            //        Publisher = "Marvel", 
            //        SeriesTitle = "Black Widow", 
            //        Years = "2020-",
            //        IssueTitle = "#5", 
            //        PublisherIcon = "M", 
            //        Color = Color.Error, 
            //        Progress = 83, 
            //        ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/9/70/6026d186cfbc7.jpg",
            //        ReadUrl = null
            //    },
            //    new Book {
            //        Id = 2,
            //        Publisher = "Marvel",
            //        SeriesTitle = "Captain Marvel",
            //        Years = "2019-",
            //        IssueTitle = "#5",
            //        PublisherIcon = "M",
            //        Color = Color.Error,
            //        Progress = 15,
            //        ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/f/10/5ccb393ea5cb4.jpg",
            //        ReadUrl = "https://read.marvel.com/#/book/51399"
            //    },
            //    new Book {
            //        Id = 3,
            //        Publisher = "Marvel",
            //        SeriesTitle = "Deadpool",
            //        Years = "2008-2012",
            //        IssueTitle = "#33.1",
            //        PublisherIcon = "M",
            //        Color = Color.Error,
            //        Progress = 50,
            //        ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/6/50/5739dfbe27200.jpg",
            //        ReadUrl = "https://read.marvel.com/#/book/19855"
            //    },
            //    new Book {
            //        Id = 4,
            //        Publisher = "DC",
            //        SeriesTitle = "Harley Quinn",
            //        Years = "2016-",
            //        IssueTitle = "Vol. 1",
            //        PublisherIcon = "DC",
            //        Color = Color.Info,
            //        Progress = 50,
            //        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg",
            //        ReadUrl = "https://www.comixology.co.uk/comic-reader/74153/439438"
            //    }
            //};
        }
    }
}
