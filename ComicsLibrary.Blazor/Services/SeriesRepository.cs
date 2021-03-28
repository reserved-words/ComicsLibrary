using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly HttpClient _httpClient;

        private readonly Dictionary<Shelf, List<LibrarySeries>> _cache = new Dictionary<Shelf, List<LibrarySeries>>();

        public SeriesRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LibrarySeries>> GetShelf(Shelf shelf, bool refreshCache)
        {
            if (refreshCache)
            {
                _cache.Remove(shelf);
            }

            if (!_cache.TryGetValue(shelf, out List<LibrarySeries> series))
            {
                var shelfId = (int)shelf;
                var url = $"http://localhost:58281/Library/Shelf?shelf={shelfId}";
                var result = await _httpClient.GetFromJsonAsync<LibrarySeries[]>(url);
                series = result.ToList();
                _cache.Add(shelf, series);
            }

            return series;
        }
    }
}
