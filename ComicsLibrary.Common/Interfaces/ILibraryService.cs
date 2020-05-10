using System.Collections.Generic;
using ComicsLibrary.Common.Data;

namespace ComicsLibrary.Common
{
    public interface ILibraryService
    {
        List<NextComicInSeries> GetAllNextIssues();
        void UpdateHomeBookType(HomeBookType homeBookType);
        int GetProgress(int seriesId);
        NextComicInSeries GetNextUnread(int seriesId);
        List<LibrarySeries> GetShelf(SeriesStatus status);
        List<LibraryShelf> GetShelves();
        LibrarySeries GetSeries(int id);
    }
}
