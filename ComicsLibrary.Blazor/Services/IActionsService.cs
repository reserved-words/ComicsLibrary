using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Services
{
    public interface IActionsService
    {
        List<SeriesAction> GetActions(Shelf shelf);
    }
}
