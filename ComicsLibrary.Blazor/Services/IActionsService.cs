using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Services
{
    public interface IActionsService
    {
        List<SeriesAction> GetSeriesActions(Shelf? shelf, bool includeView);
        List<BookAction> GetBookActions();
        List<BooklistAction> GetBooklistActions();
    }
}
