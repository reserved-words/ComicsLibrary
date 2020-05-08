using ComicsLibrary.Common.Models;

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
    }
}
