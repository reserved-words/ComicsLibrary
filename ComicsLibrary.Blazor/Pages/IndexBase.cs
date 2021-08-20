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
        private IReadingRepository _repository { get; set; }

        [Inject]
        private IMessenger _messenger { get; set; }

        public List<NextComicInSeries> Books { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var books = await _repository.GetNextToRead(false);


            Books = books.ToList();
        }

        protected async Task<bool> SkipNext(NextComicInSeries book)
        {
            var nextBook = await _repository.MoveNext(book);

            Replace(book, nextBook);

            return true;
        }

        protected async Task<bool> SkipPrevious(NextComicInSeries book)
        {
            var previousBook = await _repository.MovePrevious(book);

            Replace(book, previousBook);

            return true;
        }

        private void Replace(NextComicInSeries oldBook, NextComicInSeries newBook)
        {
            var index = Books.IndexOf(oldBook);

            Books.Remove(oldBook);

            Books.Insert(index, newBook);

            StateHasChanged();
        }
    }
}
