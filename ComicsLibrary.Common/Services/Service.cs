using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;
using Series = ComicsLibrary.Common.Models.Series;

using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Services
{
    public class Service : IService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IMapper _mapper;
        private readonly IApiService _apiService;
        private readonly ILogger _logger;

        public Service(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper,
            IApiService apiService, ILogger logger)
        {
            _apiService = apiService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void AbandonSeries(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>().GetById(id);
                series.Abandoned = true;
                uow.Save();
            }
        }

        public async Task<int> AddSeriesToLibrary(ApiSeries series)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var s = _mapper.Map<ApiSeries, Series>(series);
                    s.LastUpdated = DateTime.Now;
                    s.Books = await _apiService.GetAllSeriesComicsAsync(series.SourceItemID);
                    foreach (var c in s.Books)
                    {
                        c.DateAdded = DateTime.Now;
                    }
                    uow.Repository<Series>().Insert(s);
                    uow.Save();
                    return s.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return 0;
            }
        }

        public ApiSeries GetSeries(int seriesId, int numberOfComics)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var dbSeries = uow.Repository<Series>()
                    .Including(s => s.Books)
                    .Single(c => c.Id == seriesId);

                var dbBookTypes = uow.Repository<BookType>()
                    .Including(bt => bt.HomeBookTypes)
                    .Select(bt => new 
                    { 
                        ID = bt.ID, 
                        Name = bt.Name, 
                        Home = bt.HomeBookTypes.Any(t => t.SeriesId == seriesId && t.Enabled)  
                    })
                    .ToDictionary(t => t.ID, t => new { Name = t.Name, Home = t.Home });

                var series = _mapper.Map<Series, ApiSeries>(dbSeries);

                series.BookLists = dbSeries.Books
                    .GroupBy(b => b.BookTypeID)
                    .Select(g => new BookList
                    {
                        TypeId = g.Key.Value,
                        TypeName = dbBookTypes[g.Key.Value].Name,
                        TotalBooks = g.Count(),
                        Home = dbBookTypes[g.Key.Value].Home,
                        Books = g.OrderByDescending(c => c.Number)
                            .Take(numberOfComics)
                            .Select(b => _mapper.Map<Book, ApiComic>(b))
                            .ToArray()
                    })
                    .ToArray();

                return series;
            }
        }

        public List<ApiComic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return _mapper.Map<List<Book>, List<ApiComic>>(uow.Repository<Book>()
                    .Where(s => s.SeriesId == seriesId && s.BookTypeID == typeId)
                    .OrderByDescending(c => c.Number)
                    .Skip(offset)
                    .Take(limit)
                    .ToList());
            }
        }

        public List<ApiComic> GetComics(int seriesId, int limit, int offset)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return _mapper.Map<List<Book>, List<ApiComic>>(uow.Repository<Book>()
                    .Including(s => s.Series)
                    .Where(s => s.SeriesId == seriesId)
                    .OrderByDescending(c => c.Number)
                    .Skip(offset)
                    .Take(limit)
                    .ToList());
            }
        }

        public async Task<PagedResult<ApiComic>> GetComicsByMarvelId(int marvelId, int limit, int offset)
        {
            try
            {
                var page = offset / limit + 1;
                var result = await _apiService.GetSeriesComicsAsync(marvelId, limit, page);
                var comics = _mapper.Map<List<Book>, List<ApiComic>>(result.Results);
                return new PagedResult<ApiComic>(comics, limit, page, result.Total);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public List<ApiSeries> GetSeriesByStatus(SeriesStatus status)
        {
            using (var uow = _unitOfWorkFactory())
            {
                // TODO - change to prevent fetching all comics from each series - only need the first

                var series = status == SeriesStatus.Reading
                    ? GetSeriesInProgress(uow)
                    : status == SeriesStatus.ToRead
                    ? GetSeriesToRead(uow)
                    : status == SeriesStatus.Read
                    ? GetSeriesFinished(uow)
                    : GetSeriesArchived(uow);

                var list = series.OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();

                if (status == SeriesStatus.ToRead)
                {
                    list.ForEach(s => s.Progress = 0);
                }
                else if (status == SeriesStatus.Read)
                {
                    list.ForEach(s => s.Progress = 100);
                }

                return list;
            }
        }

        public NextComicInSeries MarkAsRead(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.DateRead = DateTime.Now;
                uow.Save();

                var next = uow.Repository<Book>()
                    .Including(c => c.Series)
                    .Where(c => c.SeriesId == comic.SeriesId && !c.DateRead.HasValue)
                    .OrderBy(c => c.OnSaleDate)
                    .FirstOrDefault();

                if (next == null)
                    return null;

                return _mapper.Map<Book, NextComicInSeries>(next);
            }
        }

        public void MarkAsUnread(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.DateRead = null;
                uow.Save();
            }
        }

        public void ReinstateSeries(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>().GetById(id);
                series.Abandoned = false;
                uow.Save();
            }
        }


        public void RemoveSeriesFromLibrary(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comicsToDelete = uow.Repository<Book>().Where(c => c.SeriesId == id).Select(c => c.Id).ToList();

                foreach (var comic in comicsToDelete)
                {
                    uow.Repository<Book>().Delete(comic);
                }

                uow.Repository<Series>().Delete(id);
                uow.Save();
            }
        }

        public async Task UpdateSeries(int numberToUpdate)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var weekAgo = DateTime.Now.AddDays(-7);
                    var yearAgo = DateTime.Now.AddYears(-1);

                    var ongoingSeries = uow.Repository<Series>()
                        .Where(s => s.SourceItemID.HasValue
                            && s.LastUpdated < weekAgo
                            && (s.Books.Any(c => c.OnSaleDate > yearAgo)
                                || s.Books.Any(c => string.IsNullOrEmpty(c.ReadUrl))));

                    var totalOngoingSeries = ongoingSeries.Count();

                    if (totalOngoingSeries == 0)
                        return;

                    var seriesToUpdate = ongoingSeries
                        .OrderBy(s => s.LastUpdated)
                        .Take(numberToUpdate)
                        .ToList();

                    foreach (var series in seriesToUpdate)
                    {
                        await UpdateSeries(uow, series);
                    }

                    uow.Save();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public async Task<PagedResult<ApiSeries>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            var searchResults = await _apiService.SearchSeriesAsync(title, limit, page, (SearchOrder)sortOrder);

            using (var uow = _unitOfWorkFactory())
            {
                var inLibrary = uow.Repository<Series>()
                    .Where(s => s.SourceItemID.HasValue)
                    .ToDictionary(s => s.SourceItemID.Value, s => s.Id);

                var series = new List<ApiSeries>();
                foreach (var result in searchResults.Results)
                {
                    if (!result.SourceItemID.HasValue)
                        continue;

                    inLibrary.TryGetValue(result.SourceItemID.Value, out int libraryId);

                    series.Add(new ApiSeries
                    {
                        SourceItemID = result.SourceItemID.Value,
                        Title = result.Title,
                        StartYear = result.StartYear,
                        EndYear = result.EndYear,
                        Type = result.Type,
                        ImageUrl = result.ImageUrl,
                        Id = libraryId,
                        Url = result.Url
                    });
                }
                return new PagedResult<ApiSeries>(series, limit, page, searchResults.Total);
            }
        }

        private async Task UpdateSeries(IUnitOfWork uow, Series series)
        {
            try
            {
                var newSeriesUpdateTime = DateTime.Now;
                var updatedComics = await _apiService.GetAllSeriesComicsAsync(series.SourceItemID.Value);
                foreach (var comic in updatedComics)
                {
                    UpdateComic(uow, comic, series.Id);
                }
                series.LastUpdated = newSeriesUpdateTime;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", series.Id);
                _logger.Log(ex);
            }
        }

        private void UpdateComic(IUnitOfWork uow, Book comic, int seriesId)
        {
            try
            {
                if (string.IsNullOrEmpty(comic.ImageUrl) || comic.Title.EndsWith("Variant)"))
                    return;

                var savedComic = uow.Repository<Book>().SingleOrDefault(c => c.SourceItemID == comic.SourceItemID);

                if (savedComic == null)
                {
                    comic.SeriesId = seriesId;
                    comic.DateAdded = DateTime.Now;
                    uow.Repository<Book>().Insert(comic);
                }
                else
                {
                    _mapper.Map(comic, savedComic);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                ex.Data.Add("Comic Issue Number", comic.Number.HasValue ? comic.Number.ToString() : "null");
                ex.Data.Add("Comic Marvel ID", comic.SourceItemID.HasValue ? comic.SourceItemID.ToString() : "null");
                _logger.Log(ex);
            }
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Book>()
                    .Including(c => c.Series.HomeBookTypes)
                    .Where(c => !c.Series.Abandoned 
                        && !c.DateRead.HasValue
                        && c.Series.HomeBookTypes.Any(t => t.BookTypeId == c.BookTypeID && t.Enabled))
                    .OrderBy(c => c.OnSaleDate)
                    .ToList();

                var groups = series
                     .GroupBy(c => c.SeriesId)
                     .Select(g => new { Unread = g.Count(), Next = g.First() })
                     .OrderBy(c => c.Next.Series.Title)
                     .ToList();

                var list = new List<NextComicInSeries>();

                foreach (var g in groups)
                {
                    var comic = _mapper.Map<Book, NextComicInSeries>(g.Next);
                    comic.UnreadIssues = g.Unread;
                    list.Add(comic);
                }

                return list;
            }
        }

        private IEnumerable<Series> GetSeriesInProgress(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Books).Where(s => !s.Abandoned && s.Books.Any(c => c.DateRead.HasValue) && s.Books.Any(c => !c.DateRead.HasValue));
        }

        private IEnumerable<Series> GetSeriesToRead(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Books).Where(s => !s.Abandoned && s.Books.All(c => !c.DateRead.HasValue));
        }

        private IEnumerable<Series> GetSeriesFinished(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Books).Where(s => !s.Abandoned && s.Books.All(c => c.DateRead.HasValue));
        }

        private IEnumerable<Series> GetSeriesArchived(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Books).Where(s => s.Abandoned);
        }

        public void UpdateHomeBookType(HomeBookType homeBookType)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var item = uow.Repository<HomeBookType>()
                        .SingleOrDefault(bt => bt.BookTypeId == homeBookType.BookTypeId
                            && bt.SeriesId == homeBookType.SeriesId);

                    if (item == null)
                    {
                        uow.Repository<HomeBookType>().Insert(homeBookType);
                    }
                    else
                    {
                        item.Enabled = homeBookType.Enabled;
                    }

                    uow.Save();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }

        }
    }
}
