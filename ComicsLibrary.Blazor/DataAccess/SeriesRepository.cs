using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly ILibrary _library;

        private readonly Dictionary<Shelf, List<Series>> _cache = new Dictionary<Shelf, List<Series>>();

        public SeriesRepository(ILibrary library)
        {
            _library = library;
        }

        public async Task<SeriesDetail> GetSeries(int id)
        {
            return await _library.GetSeries(id);
        }

        public async Task<List<Series>> GetShelf(Shelf? shelf, bool refreshCache)
        {
            if (shelf.HasValue)
            {
                if (refreshCache)
                {
                    _cache.Remove(shelf.Value);
                }

                return await GetAllSeries(shelf.Value);
            }
            else
            {
                if (refreshCache)
                {
                    _cache.Clear();
                }

                var allShelves = Enum.GetValues(typeof(Shelf)).OfType<Shelf>();

                var allSeries = new List<Series>();

                foreach (var sh in allShelves)
                {
                    var shelfSeries = await GetAllSeries(sh);
                    allSeries.AddRange(shelfSeries);
                }

                return allSeries
                    .OrderBy(s => s.Title)
                    .ThenBy(s => s.Years)
                    .ToList();
            }
        }

        private async Task<List<Series>> GetAllSeries(Shelf shelf)
        {
            if (!_cache.TryGetValue(shelf, out List<Series> seriesList))
            {
                seriesList = await _library.GetShelf((int)shelf);
                _cache.Add(shelf, seriesList);
            }

            return seriesList;
        }

        public async Task<bool> UpdateShelf(Series series, Shelf newShelf)
        {
            var success = await _library.UpdateShelf(series.Id, (int)newShelf);

            if (success)
            {
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

                return true;
            }

            // Log error

            return false;
        }
    }
}
