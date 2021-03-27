using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class ArchivedBase : ShelfBase
    {
        public override Shelf Shelf => Shelf.Archived;

    }
}
