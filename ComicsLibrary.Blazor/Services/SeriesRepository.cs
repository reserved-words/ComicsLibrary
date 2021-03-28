using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Services
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly HttpClient _httpClient;

        private readonly Dictionary<Shelf, List<Series>> _cache = new Dictionary<Shelf, List<Series>>();

        public SeriesRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Series>> GetShelf(Shelf shelf, bool refreshCache)
        {
            if (refreshCache)
            {
                _cache.Remove(shelf);
            }

            if (!_cache.TryGetValue(shelf, out List<Series> items))
            {
                var shelfId = (int)shelf;
                var url = $"http://localhost:58281/Library/Shelf?shelf={shelfId}";
                var result = await _httpClient.GetFromJsonAsync<LibrarySeries[]>(url);
                items = result.Select(b => new Series(b)).ToList();
                _cache.Add(shelf, items);
            }

            return items;
        }

        public async Task UpdateShelf(Series series, Shelf newShelf)
        {
            // Call the relevant API method - should now just mean updating the Shelf column?

            if (_cache.ContainsKey(series.Shelf))
            {
                _cache[series.Shelf].Remove(series);
            }

            series.Shelf = newShelf;

            if (_cache.ContainsKey(newShelf))
            {
                _cache[series.Shelf].Add(series);
                _cache[series.Shelf] = _cache[series.Shelf].OrderBy(s => s.Title).ToList();
            }
        }
    }
}
