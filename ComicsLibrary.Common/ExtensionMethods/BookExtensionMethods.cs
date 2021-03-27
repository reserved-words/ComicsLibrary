using ComicsLibrary.Common.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace ComicsLibrary.Common
{
    public static class BookExtensionMethods
    {
        public static string GetBookTitle(this Book book)
        {
            return book.BookTypeID == 1
                ? $"#{book.Number}"
                : book.BookTypeID == 2
                ? $"Vol. {book.Number}"
                : "";
        }

        public static string GetBookTitle(this SeriesBook book)
        {
            return book.BookTypeID == 1
                ? $"#{book.Number}"
                : book.BookTypeID == 2
                ? $"Vol. {book.Number}"
                : "";
        }


        public static (string Title, string Years) SplitSeriesTitle(this NextComicInSeries book)
        {
            var pattern = @"^(.+)\s*\(([0-9]+)\s*\-\s*(.*)\)$";
            var regex = new Regex(pattern);
            var matches = regex.Matches(book.SeriesTitle);

            if (!matches.Any())
                return (book.SeriesTitle, null);

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
