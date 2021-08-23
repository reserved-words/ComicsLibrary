using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Services
{
    public interface ISeriesActionsService
    {
        List<SeriesAction> GetActions(Shelf? shelf, bool includeView);
    }
}
