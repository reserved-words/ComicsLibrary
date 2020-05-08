using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common
{
    public interface ILibrary
    {
        List<NextComicInSeries> GetAllNextIssues();
        NextComicInSeries GetNextUnread(int seriesId);
        List<LibrarySeries> GetSeries();
        LibrarySeries GetSeries(int seriesId);
        void UpdateHomeBookType(HomeBookType homeBookType);
    }
}
