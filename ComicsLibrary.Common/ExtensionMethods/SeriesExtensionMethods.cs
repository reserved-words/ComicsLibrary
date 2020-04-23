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
            var validBooks = series.GetValidBooks();

            var read = validBooks.Count(c => c.DateRead.HasValue && !c.Hidden);
            var total = validBooks.Count(c => !c.Hidden);

            return total == 0 ? 0 : (int)Math.Round(100 * (double)read / total);
        }
    }
}
