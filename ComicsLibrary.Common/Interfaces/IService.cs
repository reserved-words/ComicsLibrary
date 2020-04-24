using System.Collections.Generic;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IService
    {
        List<NextComicInSeries> GetAllNextIssues();
        void ReinstateSeries(int id);
        void ArchiveSeries(int id);
        void RemoveSeriesFromLibrary(int id);
        Api.Series GetSeries(int id, int numberOfComics);
        List<Api.Comic> GetBooks(int seriesId, int typeId, int limit, int offset);
        List<Api.Series> GetSeriesByStatus(SeriesStatus status);
        void UpdateHomeBookType(HomeBookType homeBookType);
        int GetProgress(int seriesId);
        NextComicInSeries GetNextUnread(int seriesId);
    }
}
