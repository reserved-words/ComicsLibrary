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
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly ISeriesUpdater _updater;

        public SearchService(Func<int, ISourceSearcher> serviceFactory, Func<int, ISourceUpdateService> updaterFactory, 
            Func<IUnitOfWork> unitOfWorkFactory, ISeriesUpdater updater)
        {
            _searcherFactory = serviceFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _updaterFactory = updaterFactory;
            _updater = updater;
        }

        public async Task<int> AddToLibrary(SearchResult searchResult)
        {
            var series = new Data.Series
            {
                SourceId = searchResult.SourceId,
                SourceItemID = searchResult.SourceItemId,
                Title = searchResult.Title,
                Url = searchResult.Url,
                ImageUrl = searchResult.ImageUrl
            };

            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Data.Series>().Insert(series);
                uow.Save();
            }

            var updateTime = DateTime.Now;

            var updatedSeries = await _updaterFactory(searchResult.SourceId)
                .GetUpdatedSeries(searchResult.SourceItemId, searchResult.Url, false);

            _updater.UpdateSeries(series.Id, updateTime, updatedSeries);

            return series.Id;
        }

        public async Task<PagedResult<Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset)
        {
            var results = await _searcherFactory(sourceID).GetBooks(sourceItemID, limit, offset);
            throw new NotImplementedException();
        }

        public async Task<PagedResult<SearchResult>> SearchByTitle(int sourceID, string title, int sortOrder, int limit, int page)
        {
            var results = await _searcherFactory(sourceID).SearchByTitle(title, sortOrder, limit, page);

            using (var uow = _unitOfWorkFactory())
            {
                var inLibrary = uow.Repository<Common.Data.Series>()
                    .Where(s => s.SourceId == sourceID)
                    .ToDictionary(s => s.SourceItemID.Value, s => s.Id);

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
}
