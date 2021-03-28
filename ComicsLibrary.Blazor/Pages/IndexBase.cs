using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IReadingRepository Repository { get; set; }

        public List<NextComicInSeries> Books { get; set; } = new List<NextComicInSeries>();

        protected override async Task OnInitializedAsync()
        {
            var books = await Repository.GetNextToRead(false);

            foreach (var book in books)
            {
                (var title, var years) = book.SplitSeriesTitle();
                book.SeriesTitle = title;
                book.Years = years;
            }

            Books = books.ToList();
        }
    }
}
