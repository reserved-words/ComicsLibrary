using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ComicsLibrary.Common
{
    public static class SeriesExtensionMethods
    {
        public static int GetProgress(this Series series)
        {
            var validBooks = series.GetValidBooks();

            var read = validBooks.Count(c => c.DateRead.HasValue && !c.Hidden);
            var total = validBooks.Count(c => !c.Hidden);

            return total == 0 ? 0 : (int)Math.Round(100 * (double)read / total);
        }


        public static string YearsActive(this Series series)
        {
            if (series.StartYear == null)
                return series.EndYear?.ToString();

            if (series.EndYear == null || series.StartYear == series.EndYear)
                return series.StartYear.ToString();

            return $"{series.StartYear} - {series.EndYear}";
        }

        public static IEnumerable<Book> GetValidBooks(this Series series)
        {
            if (series.HomeBookTypes == null || series.Books == null)
                return new List<Book>();

            var validTypes = series.HomeBookTypes
                .Where(bt => bt.Enabled)
                .Select(bt => bt.BookTypeId)
                .ToList();

            return series.Books
                .Where(b => !b.Hidden
                    && b.BookTypeID.HasValue
                    && validTypes.Contains(b.BookTypeID.Value));

        }

        public static string GetImageUrl(this Series series)
        {
            return series.GetValidBooks()
                .OrderBy(b => b.Number)
                .FirstOrDefault()?.ImageUrl ?? series.ImageUrl;
        }


        public static (string Title, string Years) SplitSeriesTitle(this LibrarySeries series)
        {
            var pattern = @"^(.+)\s*\(([0-9]+)\s*\-\s*(.*)\)$";
            var regex = new Regex(pattern);
            var matches = regex.Matches(series.Title);

            if (!matches.Any())
                return (series.Title, null);

            var match = matches[0];
            var title = match.Groups[1].Value;
            var startYear = match.Groups[2].Value;
            var endYear = int.TryParse(match.Groups[3].Value, out int result)
                ? match.Groups[3].Value
                : "";

            return (title, $"{startYear}-{endYear}");
        }
    }
}
