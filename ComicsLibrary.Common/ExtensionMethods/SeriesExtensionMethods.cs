using ComicsLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComicsLibrary.Common
{
    public static class SeriesExtensionMethods
    {
        public static int GetProgress(this Series series)
        {
            var validTypes = series.HomeBookTypes
                .Where(bt => bt.Enabled)
                .Select(bt => bt.BookTypeId)
                .ToList();

            var validBooks = series.Books
                .Where(b => !b.Hidden
                    && b.BookTypeID.HasValue
                    && validTypes.Contains(b.BookTypeID.Value));

            var read = validBooks.Count(c => c.DateRead.HasValue && !c.Hidden);
            var total = validBooks.Count(c => !c.Hidden);

            return (int)Math.Round(100 * (double)read / total);
        }
    }
}
