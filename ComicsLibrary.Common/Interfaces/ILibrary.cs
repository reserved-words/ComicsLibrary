using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Common
{
    public interface ILibrary
    {
        List<NextComicInSeries> GetAllNextIssues();
        NextComicInSeries GetNextUnread(int seriesId);
        List<LibrarySeries> GetSeries(Shelf status);
        List<LibrarySeries> GetSeries();
        LibrarySeries GetSeries(int seriesId);
        void UpdateHomeBookType(HomeBookType homeBookType);
        void UpdateSeriesShelf(int id, int shelf);
    }
}
