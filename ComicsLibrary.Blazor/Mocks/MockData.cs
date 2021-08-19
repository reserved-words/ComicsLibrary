using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Blazor.Mocks
{
    public static class MockData
    {
        public static readonly List<LibrarySeries> AllSeries;
        public static readonly Dictionary<int, List<Comic>> AllBooks;

        static MockData()
        {
            AllSeries = new List<LibrarySeries>();
            AllBooks = new Dictionary<int, List<Comic>>();
            PopulateSeries();
        }

        private static void PopulateSeries()
        {
            AllSeries.AddSeries(Shelf.Reading, "M", "Black Widow", 2020, null, "https://i.annihil.us/u/prod/marvel/i/mg/c/40/5f3d36dc73d2a.jpg")
                .AddBook("#1", true, "https://i.annihil.us/u/prod/marvel/i/mg/c/40/5f3d36dc73d2a.jpg")
                .AddBook("#2", true, "https://i.annihil.us/u/prod/marvel/i/mg/6/f0/5f735c594a0e9.jpg")
                .AddBook("#3", true, "https://i.annihil.us/u/prod/marvel/i/mg/5/b0/601af793d000e.jpg")
                .AddBook("#4", true, "https://i.annihil.us/u/prod/marvel/i/mg/2/80/601afd2412732.jpg")
                .AddBook("#5", false, "https://i.annihil.us/u/prod/marvel/i/mg/9/70/6026d186cfbc7.jpg", "https://read.marvel.com/#/book/56005")
                .AddBook("#6", false, "https://i.annihil.us/u/prod/marvel/i/mg/9/20/607717bae5be1.jpg")
                .AddBook("#7", false, "https://i.annihil.us/u/prod/marvel/i/mg/b/d0/60afe2f70cd22.jpg")
                .AddBook("#8", false, "https://i.annihil.us/u/prod/marvel/i/mg/4/50/609ece2894980.jpg")
                .AddBook("#9", false, "https://i.annihil.us/u/prod/marvel/i/mg/9/20/60fad343bf338.jpg")
                .AddBook("#10", false, "https://i.annihil.us/u/prod/marvel/i/mg/6/10/60e5e16d261c4.jpg");

            AllSeries.AddSeries(Shelf.ToReadNext, "DC", "Harley Quinn", 2016, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg")
                .AddBook("Vol. 1", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg", "https://www.comixology.co.uk/comic-reader/74153/439438")
                .AddBook("Vol. 2", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/531028/531028._SX312_QL80_TTD_.jpg")
                .AddBook("Vol. 3", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/572264/572264._SX312_QL80_TTD_.jpg")
                .AddBook("Vol. 4", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/618830/618830._SX312_QL80_TTD_.jpg")
                .AddBook("Vol. 5", false, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/651046/651046._SX312_QL80_TTD_.jpg");

            AllSeries.AddSeries(Shelf.Finished, "M", "Alias", 2001, 2003, "https://i.annihil.us/u/prod/marvel/i/mg/4/20/56966d674b06d.jpg");

            AllSeries.AddSeries(Shelf.Archived, "DC", "Action Comics", 2016, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439423/439423._SX312_QL80_TTD_.jpg");

            AllSeries.AddSeries(Shelf.Archived, "I", "Bitch Planet", null, null, "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/294324/294324._SX360_QL80_TTD_.jpg");
        }

        public static void UpdateSeriesProgress(int seriesId)
        {
            var series = GetSeries(seriesId);
            var comics = AllBooks[seriesId];

            var progress = 100 * (double)comics.Count(c => c.IsRead) / (double)comics.Count;

            series.Progress = (int)Math.Round(progress, 0);
        }

        public static LibrarySeries GetSeries(int seriesId)
        {
            return AllSeries
                .Single(c => c.Id == seriesId);
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

            MockData.AllBooks.Add(id, new List<Comic>());

            return series;
        }

        public static LibrarySeries AddBook(this LibrarySeries series, string title, bool read, string imageUrl, string readUrl = null)
        {
            var numberOfComicsAdded = MockData.AllBooks.Sum(s => s.Value.Count);

            var comic = new Comic
            {
                Id = numberOfComicsAdded + 1,
                SeriesId = series.Id,
                IssueTitle = title,
                SeriesTitle = series.Title,
                IsRead = read,
                ImageUrl = imageUrl,
                ReadUrl = readUrl
            };

            MockData.AllBooks[series.Id].Add(comic);

            MockData.UpdateSeriesProgress(series.Id);

            return series;
        }

    }
}
