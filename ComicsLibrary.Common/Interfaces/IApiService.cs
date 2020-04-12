using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IApiService
    {
        Task<ApiResult<Models.Series>> SearchSeriesAsync(string titleStartsWith, int limit, int page, SearchOrder? orderBy);

        Task<ApiResult<Book>> GetSeriesComicsAsync(int id, int maxResults, int page);
        Task<List<Book>> GetAllSeriesComicsAsync(int id);
    }
}