using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IService
    {
        List<NextComicInSeries> GetAllNextIssues();
        void MarkAsUnread(int[] ids);
        void MarkAsRead(int[] ids);
        NextComicInSeries MarkAsRead(int id);
        void ReinstateSeries(int id);
        void AbandonSeries(int id);
        void RemoveSeriesFromLibrary(int id);
        Task<int> AddSeriesToLibrary(Series series);
        Series GetSeries(int id, int numberOfComics);
        List<Comic> GetComics(int seriesId, int limit, int offset);
        Task UpdateSeries(int maxNumberToUpdate);
        Task<PagedResult<Series>> SearchByTitle(string title, int sortOrder, int limit, int page);
        Task<PagedResult<Comic>> GetComicsByMarvelId(int marvelId, int limit, int offset);
        
        List<Series> GetSeriesInProgress();
        List<Series> GetSeriesToRead();
        List<Series> GetSeriesFinished();
        List<Series> GetSeriesAbandoned();
    }
}
