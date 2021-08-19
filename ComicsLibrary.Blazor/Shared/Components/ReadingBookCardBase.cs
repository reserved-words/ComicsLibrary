using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class ReadingBookCardBase : ComponentBase
    {
        [Parameter]
        public NextComicInSeries Book { get; set; }

        [Parameter]
        public Func<NextComicInSeries, Task<bool>> SkipNext { get; set; }

        [Parameter]
        public Func<NextComicInSeries, Task<bool>> SkipPrevious { get; set; }

        public bool PreventSkipPrevious { get; set; }

        public bool PreventSkipNext { get; set; }

        protected override void OnParametersSet()
        {
            PreventSkipPrevious = Book.Progress == 0;
            PreventSkipNext = Book.UnreadBooks == 1;

            StateHasChanged();
        }

        protected async Task<bool> SkipNext1()
        {
            return await SkipNext(Book);
        }

        protected async Task<bool> SkipPrevious1()
        {
            return await SkipPrevious(Book);
        }
    }
}
