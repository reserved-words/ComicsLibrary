using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockSeriesRepository : Services.ISeriesRepository
    {
        public static readonly List<LibrarySeries> AllSeries;
        public static readonly Dictionary<int, List<Comic>> AllBooks;

        static MockSeriesRepository()
        {
            AllSeries = new List<LibrarySeries>();
            AllBooks = new Dictionary<int, List<Comic>>();
            PopulateSeries();
        }

        public static List<Model.Series> GetShelf(Shelf shelf)
        {
            return AllSeries
                .Where(s => s.Shelf == shelf)
                .Select(s => new Model.Series(s))
                .ToList();
        }

        public async Task<List<Model.Series>> GetShelf(Shelf shelf, bool refreshCache)
        {
            return GetShelf(shelf);
        }

        public async Task<bool> UpdateShelf(Model.Series series, Shelf newShelf)
        {
            var seriesToMove = AllSeries.Single(s => s.Id == series.Id);
            seriesToMove.Shelf = newShelf;
            if (newShelf == Shelf.Finished)
            {
                seriesToMove.Progress = 100;
            }
            else if (newShelf == Shelf.Unread)
            {
                seriesToMove.Progress = 0;
            }

            return true;
        }

        private static void PopulateSeries()
        {
            AllSeries.AddSeries(Shelf.Reading, "M", "Black Widow", 2020, null, "https://i.annihil.us/u/prod/marvel/i/mg/c/40/5f3d36dc73d2a.jpg")
                .AddBook("#1", true, "")
                .AddBook("#2", true, "")
                .AddBook("#3", true, "")
                .AddBook("#4", true, "")
                .AddBook("#5", false, "https://i.annihil.us/u/prod/marvel/i/mg/9/70/6026d186cfbc7.jpg", "https://read.marvel.com/#/book/56005");

            AllSeries.AddSeries(Shelf.ToReadNext, "DC", "Harley Quinn", 2016, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg")
                .AddBook("Vol. 1", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg", "https://www.comixology.co.uk/comic-reader/74153/439438")
                .AddBook("Vol. 2", false, "")
                .AddBook("Vol. 3", false, "")
                .AddBook("Vol. 4", false, "")
                .AddBook("Vol. 5", false, "");
            
            AllSeries.AddSeries(Shelf.Finished, "M", "Alias", 2001, 2003, "https://i.annihil.us/u/prod/marvel/i/mg/4/20/56966d674b06d.jpg");
            
            AllSeries.AddSeries(Shelf.Archived, "DC", "Action Comics", 2016, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439423/439423._SX312_QL80_TTD_.jpg");
            
            AllSeries.AddSeries(Shelf.Archived, "I", "Bitch Planet", null, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/294324/294324._SX360_QL80_TTD_.jpg");
        }
    }

    public static class HelperMethods
    {
        public static LibrarySeries AddSeries(this List<LibrarySeries> list, Shelf shelf, string publisher, string title, int? startYear, int? endYear, string imageUrl)
        {
            var id = list.Count + 1;

            var years = startYear == null
                ? ""
                : endYear == null
                ? $" ({startYear} - Present)" 
                : startYear == endYear
                ? $" ({startYear})"
                : $" ({startYear} - {endYear})";

            var fullTitle = $"{title}{years}";

            var (publisherName, colour) = publisher switch
            {
                "M" => ("Marvel Comics", "red"),
                "DC" => ("DC Comics", "blue"),
                "I" => ("Image Comics", "pink"),
                "B" => ("BOOM! Studios", "yellow"),
                _ => ("Unknown", "black")
            };

            var series = new LibrarySeries
            {
                Id = id,
                Title = fullTitle,
                ImageUrl = imageUrl,
                Progress = 0, // calculate when books are added
                Publisher = publisherName,
                PublisherIcon = publisher,
                Color = colour,
                Shelf = shelf
            };

            list.Add(series);

            MockSeriesRepository.AllBooks.Add(id, new List<Comic>());

            return series;
        }

        public static LibrarySeries AddBook(this LibrarySeries series, string title, bool read, string imageUrl, string readUrl = null)
        {
            var numberOfComicsAdded = MockSeriesRepository.AllBooks.Sum(s => s.Value.Count);

            var comic = new Comic
            {
                Id = numberOfComicsAdded + 1,
                IssueTitle = title,
                SeriesTitle = series.Title,
                IsRead = read,
                ImageUrl = imageUrl, 
                ReadUrl = readUrl
            };

            var seriesComics = MockSeriesRepository.AllBooks[series.Id];

            seriesComics.Add(comic);

            var progress = 100 * (double)seriesComics.Count(c => c.IsRead) / (double)seriesComics.Count;

            series.Progress = (int)Math.Round(progress, 0);

            return series;
        }
    }
}
