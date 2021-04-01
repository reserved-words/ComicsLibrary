using Microsoft.AspNetCore.Components;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SearchBookCardBase : ComponentBase
    {
        [Parameter]
        public Common.Data.Book Book { get; set; }
    }
}
