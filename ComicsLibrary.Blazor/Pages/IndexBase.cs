using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private IReadingRepository _repository { get; set; }

        public List<NextComicInSeries> Books { get; set; } = new List<NextComicInSeries>();

        protected override async Task OnInitializedAsync()
        {
            var books = await _repository.GetNextToRead(false);

            foreach (var book in books)
            {
                (var title, var years) = book.SplitSeriesTitle();
                book.SeriesTitle = title;
                book.Years = years;
            }

            Books = books.ToList();
        }

        protected async Task<bool> SkipNext(NextComicInSeries book)
        {
            throw new NotImplementedException();
            // DB - mark book as read (returns next book in response)
            // Update book to new values
        }

        protected async Task<bool> SkipPrevious(NextComicInSeries book)
        {
            throw new NotImplementedException();
            // DB - mark previous book as uread (returns previous book in response)
            // Update book to new values        }
        }
    }
}
