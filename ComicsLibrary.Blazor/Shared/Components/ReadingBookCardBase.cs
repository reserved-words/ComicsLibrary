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
    }
}
