using ComicsLibrary.Common.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public class SearchService : ISearchService
    {
        private readonly Func<int, ISourceSearcher> _searcherFactory;
        private readonly Func<int, ISourceUpdateService> _updaterFactory;
        private readonly ISeriesUpdater _updater;
        private readonly ISeriesRepository _repository;

        public SearchService(Func<int, ISourceSearcher> serviceFactory, Func<int, ISourceUpdateService> updaterFactory, 
            ISeriesRepository repository, ISeriesUpdater updater)
        {
            _repository = repository;
            _searcherFactory = serviceFactory;
            _updaterFactory = updaterFactory;
            _updater = updater;
        }

        public async Task<int> AddToLibrary(SearchResult searchResult)
        {
            var series = new Series
            {
                SourceId = searchResult.SourceId,
                SourceItemID = searchResult.SourceItemId,
                Title = searchResult.Title,
                Url = searchResult.Url,
                ImageUrl = searchResult.ImageUrl
            };

            var libraryId = _repository.Insert(series);

            var updateTime = DateTime.Now;

            var updatedSeries = await _updaterFactory(searchResult.SourceId)
                .GetUpdatedSeries(searchResult.SourceItemId, searchResult.Url, false);

            _updater.UpdateSeries(libraryId, updateTime, updatedSeries);

            return libraryId;
        }

        public async Task<PagedResult<Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset)
        {
            return await _searcherFactory(sourceID).GetBooks(sourceItemID, limit, offset);
        }

        public async Task<PagedResult<SearchResult>> SearchByTitle(int sourceId, string title, int sortOrder, int limit, int page)
        {
            var results = await _searcherFactory(sourceId).SearchByTitle(title, sortOrder, limit, page);

            var inLibrary = _repository.GetAllIds(sourceId)
                .ToDictionary(s => s.SourceItemId, s => s.LibraryId);

            foreach (var result in results.Results)
            {
                if (inLibrary.TryGetValue(result.SourceItemId, out int libraryId))
                {
                    result.LibraryId = libraryId;
                }
            }

            return results;
        }
    }
}
