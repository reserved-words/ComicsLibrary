using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class BookCardBase : ComponentBase
    {
        [Parameter]
        public NextComicInSeries Book { get; set; }
    }
}
