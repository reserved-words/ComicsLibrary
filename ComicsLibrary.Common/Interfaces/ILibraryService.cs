using System.Collections.Generic;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Interfaces
{
    public interface ILibraryService
    {
        List<NextComicInSeries> GetAllNextIssues();
        void UpdateHomeBookType(HomeBookType homeBookType);
        int GetProgress(int seriesId);
        NextComicInSeries GetNextUnread(int seriesId);
    }
}
