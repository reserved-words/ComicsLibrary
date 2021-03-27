using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<Model.Series> Items { get; set; } = new List<Model.Series>();

        protected override async Task OnInitializedAsync()
        {
            
        }
    }
}
