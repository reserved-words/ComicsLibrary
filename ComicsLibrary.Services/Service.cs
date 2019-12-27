using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;
using Comic = ComicsLibrary.Common.Models.Comic;
using Series = ComicsLibrary.Common.Models.Series;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Services
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
                    s.Comics = await _apiService.GetAllSeriesComicsAsync(series.MarvelId);
                    foreach (var c in s.Comics)
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

        public void AddToReadNext(int[] ids)
        {
            using (var uow = _unitOfWorkFactory())
            {
                foreach (var id in ids)
                {
                    var comic = uow.Repository<Comic>().GetById(id);
                    comic.ToReadNext = true;
                    comic.IsRead = false;
                }

                uow.Save();
            }
        }

        public ApiSeries GetSeries(int seriesId, int numberOfComics)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = _mapper.Map<Series, ApiSeries>(uow.Repository<Series>()
                    .Including(s => s.Comics)
                    .Single(c => c.Id == seriesId));

                series.Comics = series.Comics
                    .OrderByDescending(c => c.IssueNumber)
                    .Take(numberOfComics)
                    .ToArray();

                return series;
            }
        }

        public List<ApiComic> GetComics(int seriesId, int limit, int offset)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return _mapper.Map<List<Comic>, List<ApiComic>>(uow.Repository<Comic>()
                    .Including(s => s.Series)
                    .Where(s => s.SeriesId == seriesId)
                    .OrderByDescending(c => c.IssueNumber)
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
                var comics = _mapper.Map<List<Comic>, List<ApiComic>>(result.Results);
                return new PagedResult<ApiComic>(comics, limit, page, result.Total);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public List<ApiSeries> GetSeriesInProgress()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Series>()
                    .Including(s => s.Comics)
                    .Where(s => !s.Abandoned && s.Comics.Any(c => c.IsRead) && s.Comics.Any(c => !c.IsRead))
                    .OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();
            }
        }

        public List<ApiSeries> GetSeriesToRead()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>()
                    .Where(s => !s.Abandoned && s.Comics.All(c => !c.IsRead))
                    .OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();

                series.ForEach(s => s.Progress = 0);

                return series;
            }
        }

        public List<ApiSeries> GetSeriesFinished()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>()
                    .Where(s => !s.Abandoned && s.Comics.All(c => c.IsRead))
                    .OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();

                series.ForEach(s => s.Progress = 100);

                return series;
            }
        }

        public List<ApiSeries> GetSeriesAbandoned()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Series>()
                    .Including(s => s.Comics)
                    .Where(s => s.Abandoned)
                    .OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();
            }
        }

        public List<ApiComic> GetLatestAdded(int limit)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Comic>()
                    .Including(c => c.Series)
                    .OrderByDescending(c => c.DateAdded)
                    .Take(limit)
                    .ToList()
                    .Select(c => _mapper.Map<Comic, ApiComic>(c))
                    .ToList();
            }
        }

        public List<ApiSeries> GetToReadNext()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>()
                    .Including(s => s.Comics)
                    .Where(s => s.Comics.Any(c => c.ToReadNext))
                    .ToList();

                foreach (var s in series)
                {
                    s.Comics = s.Comics
                        .Where(c => c.ToReadNext)
                        .OrderByDescending(c => c.IssueNumber)
                        .ToList();
                }

                return series
                    .Select(s => _mapper.Map<Series, ApiSeries>(s))
                    .ToList();
            }
        }

        public void MarkAsRead(int[] ids)
        {
            using (var uow = _unitOfWorkFactory())
            {
                foreach (var id in ids)
                {
                    var comic = uow.Repository<Comic>().GetById(id);
                    comic.ToReadNext = false;
                    comic.IsRead = true;
                }

                uow.Save();
            }
        }

        public void MarkAsUnread(int[] ids)
        {
            using (var uow = _unitOfWorkFactory())
            {
                foreach (var id in ids)
                {
                    var comic = uow.Repository<Comic>().GetById(id);
                    comic.IsRead = false;
                }

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

        public void RemoveFromReadNext(int[] ids)
        {
            using (var uow = _unitOfWorkFactory())
            {
                foreach (var id in ids)
                {
                    var comic = uow.Repository<Comic>().GetById(id);
                    comic.ToReadNext = false;
                }

                uow.Save();
            }
        }

        public void RemoveSeriesFromLibrary(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comicsToDelete = uow.Repository<Comic>().Where(c => c.SeriesId == id).Select(c => c.Id).ToList();

                foreach (var comic in comicsToDelete)
                {
                    uow.Repository<Comic>().Delete(comic);
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
                        .Where(s => s.MarvelId.HasValue
                            && s.LastUpdated < weekAgo
                            && (s.Comics.Any(c => c.OnSaleDate > yearAgo)
                                || s.Comics.Any(c => string.IsNullOrEmpty(c.ReadUrl))));

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
                    .Where(s => s.MarvelId.HasValue)
                    .ToDictionary(s => s.MarvelId, s => s.Id);

                var series = new List<ApiSeries>();
                foreach (var result in searchResults.Results)
                {
                    if (!result.MarvelId.HasValue)
                        continue;

                    int libraryId;
                    inLibrary.TryGetValue(result.MarvelId.Value, out libraryId);

                    series.Add(new ApiSeries
                    {
                        MarvelId = result.MarvelId.Value,
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
                var updatedComics = await _apiService.GetAllSeriesComicsAsync(series.MarvelId.Value);
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

        private void UpdateComic(IUnitOfWork uow, Comic comic, int seriesId)
        {
            try
            {
                if (string.IsNullOrEmpty(comic.ImageUrl) || comic.Title.EndsWith("Variant)"))
                    return;
                    
                var savedComic = uow.Repository<Comic>().SingleOrDefault(c => c.MarvelId == comic.MarvelId);

                if (savedComic == null)
                {
                    comic.SeriesId = seriesId;
                    comic.DateAdded = DateTime.Now;
                    uow.Repository<Comic>().Insert(comic);
                }
                else
                {
                    _mapper.Map(comic, savedComic);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                ex.Data.Add("Comic Issue Number", comic.IssueNumber.HasValue ? comic.IssueNumber.ToString() : "null");
                ex.Data.Add("Comic Marvel ID", comic.MarvelId.HasValue ? comic.MarvelId.ToString() : "null");
                _logger.Log(ex);
            }
        }

        public List<ApiComic> GetLatestUpdated(int limit)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Comic>()
                    .Including(c => c.Series)
                    .Where(c =>  c.ReadUrlAdded.HasValue)
                    .OrderByDescending(c => c.ReadUrlAdded.Value)
                    .Take(limit)
                    .ToList()
                    .Select(c => _mapper.Map<Comic, ApiComic>(c))
                    .ToList();
            }
        }
    }
}
