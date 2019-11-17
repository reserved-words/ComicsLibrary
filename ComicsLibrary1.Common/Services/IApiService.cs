using ComicsLibrary.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Services
{
    public interface IApiService
    {
        Task<ApiResult<Character>> SearchCharacterAsync(string nameStartsWith, int limit, int page);

        Task<ApiResult<Series>> SearchSeriesAsync(string titleStartsWith, int limit, int page, SearchOrder? orderBy);

        Task<ApiResult<Series>> GetCharacterSeriesAsync(int id, int limit, int page);

        Task<ApiResult<Comic>> GetSeriesComicsAsync(int id, int maxResults, int page);

        Task<List<Comic>> GetAllSeriesComicsAsync(int id);

        Task<string> GetAllSeriesCharactersAsync(int id);
    }
}