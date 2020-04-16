using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IService
    {
        List<NextComicInSeries> GetAllNextIssues();
        void MarkAsUnread(int id);
        NextComicInSeries MarkAsRead(int id);
        void ReinstateSeries(int id);
        void AbandonSeries(int id);
        void RemoveSeriesFromLibrary(int id);
        Api.Series GetSeries(int id, int numberOfComics);
        List<Api.Comic> GetBooks(int seriesId, int typeId, int limit, int offset);
        List<Api.Comic> GetComics(int seriesId, int limit, int offset);
        List<Api.Series> GetSeriesByStatus(SeriesStatus status);
        void UpdateHomeBookType(HomeBookType homeBookType);
    }
}
