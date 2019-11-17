using ComicsLibrary.Common.Models;
using ComicsLibrary.Common.Interfaces;
using System;
using System.Linq;
using Comic = ComicsLibrary.Common.Models.Comic;

namespace ComicsLibrary.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IMapper _mapper;
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly IAsyncHelper _asyncHelper;

        public UpdateService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper,
            IApiService apiService, ILogger logger, IAsyncHelper asyncHelper)
        {
            _apiService = apiService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _asyncHelper = asyncHelper;
        }

        public void UpdateSeries()
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var seriesId = GetNextSeriesToUpdate(uow);

                    if (!seriesId.HasValue || seriesId == 0)
                        return;

                    UpdateSeries(uow, seriesId.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        private int? GetNextSeriesToUpdate(IUnitOfWork uow)
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var yearAgo = DateTime.Now.AddYears(-1);

            return uow.Repository<Series>()
                .Where(s => s.MarvelId.HasValue
                    && s.LastUpdated < weekAgo
                    && (s.Comics.Any(c => c.OnSaleDate > yearAgo)
                        || s.Comics.Any(c => string.IsNullOrEmpty(c.ReadUrl))))
                .OrderBy(s => s.LastUpdated)
                .Select(s => s.Id)
                .FirstOrDefault();
        }

        private void UpdateSeries(IUnitOfWork uow, int seriesId)
        {
            try
            {
                var newSeriesUpdateTime = DateTime.Now;
                var series = uow.Repository<Series>().Single(s => s.Id == seriesId);

                var comics = _asyncHelper.RunSync(() => _apiService.GetAllSeriesComicsAsync(series.MarvelId.Value));
                       
                foreach (var comic in comics)
                {
                    TryUpdateComic(uow, comic, series.Id);
                }
                series.LastUpdated = newSeriesUpdateTime;
                uow.Save();
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                _logger.Log(ex);
            }
        }

        private void TryUpdateComic(IUnitOfWork uow, Comic comic, int seriesId)
        {
            try
            {
                if (!IsValid(comic))
                    return;

                UpdateComic(uow, comic, seriesId);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                ex.Data.Add("Comic Issue Number", comic.IssueNumber.HasValue ? comic.IssueNumber.ToString() : "null");
                ex.Data.Add("Comic Marvel ID", comic.MarvelId.HasValue ? comic.MarvelId.ToString() : "null");
                _logger.Log(ex);
            }
        }

        private void UpdateComic(IUnitOfWork uow, Comic comic, int seriesId)
        {
            var savedComic = uow.Repository<Comic>()
                .SingleOrDefault(c => c.MarvelId == comic.MarvelId);

            if (savedComic == null)
            {
                AddNewComic(uow, comic, seriesId);
            }
            else
            {
                UpdateExistingComic(comic, savedComic);
            }
        }

        private static void AddNewComic(IUnitOfWork uow, Comic comic, int seriesId)
        {
            comic.SeriesId = seriesId;
            comic.DateAdded = DateTime.Now;
            uow.Repository<Comic>().Insert(comic);
        }

        private void UpdateExistingComic(Comic comic, Comic savedComic)
        {
            var readUrlUpdated = IsReadUrlUpdated(comic, savedComic);

            _mapper.Map(comic, savedComic);

            if (readUrlUpdated)
            {
                savedComic.ReadUrlAdded = DateTime.Now;
            }
        }

        private static bool IsReadUrlUpdated(Comic comic, Comic savedComic)
        {
            return string.IsNullOrEmpty(savedComic.ReadUrl) && !string.IsNullOrEmpty(comic.ReadUrl);
        }

        private bool IsValid(Comic comic)
        {
            return !string.IsNullOrEmpty(comic.ImageUrl) && !comic.Title.EndsWith("Variant)");
        }
    }
}
