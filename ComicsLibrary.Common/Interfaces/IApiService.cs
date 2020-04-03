using ComicsLibrary.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IApiService
    {
        Task<ApiResult<Series>> SearchSeriesAsync(string titleStartsWith, int limit, int page, SearchOrder? orderBy);

        Task<ApiResult<Comic>> GetSeriesComicsAsync(int id, int maxResults, int page);

        Task<List<Comic>> GetAllSeriesComicsAsync(int id);
    }
}