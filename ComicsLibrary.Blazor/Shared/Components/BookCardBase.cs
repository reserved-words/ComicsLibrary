using Microsoft.AspNetCore.Components;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class BookCardBase : ComponentBase
    {
        [Parameter]
        public Model.Book Book { get; set; }
    }
}
