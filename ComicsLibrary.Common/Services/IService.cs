using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;

namespace ComicsLibrary.Common.Services
{
    public interface IService
    {
        void RemoveFromReadNext(int[] ids);
        void AddToReadNext(int[] ids);
        void MarkAsUnread(int[] ids);
        void MarkAsRead(int[] ids);
        void ReinstateSeries(int id);
        void AbandonSeries(int id);
        void RemoveSeriesFromLibrary(int id);
        Task<int> AddSeriesToLibrary(Series series);
        Series GetSeries(int id, int numberOfComics);
        List<Comic> GetComics(int seriesId, int limit, int offset);
        List<Series> GetToReadNext();
        List<Comic> GetLatestAdded(int limit);
        List<Comic> GetLatestUpdated(int limit);
        Task UpdateSeries(int maxNumberToUpdate);
        Task<PagedResult<Series>> SearchByTitle(string title, int sortOrder, int limit, int page);
        Task<PagedResult<Comic>> GetComicsByMarvelId(int marvelId, int limit, int offset);
        List<Series> GetSeriesInProgress();
        List<Series> GetSeriesToRead();
        List<Series> GetSeriesFinished();
        List<Series> GetSeriesAbandoned();
    }
}
