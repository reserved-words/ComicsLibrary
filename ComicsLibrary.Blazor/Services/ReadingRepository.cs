using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class ReadingRepository : IReadingRepository
    {
        private readonly HttpClient _httpClient;

        private List<NextComicInSeries> _cache = null;

        public ReadingRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache)
        {
            if (refreshCache)
            {
                _cache = null;
            }

            if (_cache == null)
            {
                var url = "http://localhost:58281/Library/GetAllNextUnread";
                var books = await _httpClient.GetFromJsonAsync<NextComicInSeries[]>(url);
                _cache = books.ToList();
            }

            return _cache;
        }
    }
}
