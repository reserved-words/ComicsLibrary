using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor
{
    public interface ISeriesActionsService
    {
        List<SeriesAction> GetActions(Shelf? shelf, bool includeView);
    }
}
