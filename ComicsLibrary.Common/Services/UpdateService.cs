using ComicsLibrary.Common.Models;
using ComicsLibrary.Common.Interfaces;
using System;
using System.Linq;

namespace ComicsLibrary.Common.Services
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

        public void UpdateSeries(int sourceID)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var seriesId = GetNextSeriesToUpdate(uow, sourceID);

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

        private int? GetNextSeriesToUpdate(IUnitOfWork uow, int sourceID)
        {
            var weekAgo = DateTime.Now.AddDays(-7);
            var yearAgo = DateTime.Now.AddYears(-1);

            // Need to change this to check if series is finished

            return uow.Repository<Series>()
                .Where(s => s.SourceItemID.HasValue
                    && s.SourceItemID == sourceID
                    && s.LastUpdated < weekAgo
                    && !s.IsFinished
                    && (s.Books.Any(c => c.OnSaleDate > yearAgo)
                        || s.Books.Any(c => string.IsNullOrEmpty(c.ReadUrl))))
                .OrderBy(s => s.LastUpdated)
                .Select(s => s.Id)
                .FirstOrDefault();
        }

        private void UpdateSeries(IUnitOfWork uow, int seriesId)
        {
            // May need to set to IsFinished

            try
            {
                var newSeriesUpdateTime = DateTime.Now;
                var series = uow.Repository<Series>().Single(s => s.Id == seriesId);

                var comics = _asyncHelper.RunSync(() => _apiService.GetAllSeriesComicsAsync(series.SourceItemID.Value));
                       
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

        private void TryUpdateComic(IUnitOfWork uow, Book book, int seriesId)
        {
            try
            {
                if (!IsValid(book))
                    return;

                UpdateComic(uow, book, seriesId);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                ex.Data.Add("Comic Issue Number", book.Number.HasValue ? book.Number.ToString() : "null");
                ex.Data.Add("Comic Source Item ID", book.SourceItemID.HasValue ? book.SourceItemID.ToString() : "null");
                _logger.Log(ex);
            }
        }

        private void UpdateComic(IUnitOfWork uow, Book comic, int seriesId)
        {
            var savedComic = uow.Repository<Book>()
                .SingleOrDefault(c => c.SourceItemID == comic.SourceItemID && c.SeriesId == seriesId);

            if (savedComic == null)
            {
                AddNewComic(uow, comic, seriesId);
            }
            else
            {
                UpdateExistingComic(comic, savedComic);
            }
        }

        private static void AddNewComic(IUnitOfWork uow, Book comic, int seriesId)
        {
            comic.SeriesId = seriesId;
            comic.DateAdded = DateTime.Now;
            uow.Repository<Book>().Insert(comic);
        }

        private void UpdateExistingComic(Book comic, Book savedComic)
        {
            var readUrlUpdated = IsReadUrlUpdated(comic, savedComic);

            _mapper.Map(comic, savedComic);

            if (readUrlUpdated)
            {
                savedComic.ReadUrlAdded = DateTime.Now;
            }
        }

        private static bool IsReadUrlUpdated(Book comic, Book savedComic)
        {
            return string.IsNullOrEmpty(savedComic.ReadUrl) && !string.IsNullOrEmpty(comic.ReadUrl);
        }

        private bool IsValid(Book comic)
        {
            return !string.IsNullOrEmpty(comic.ImageUrl) && !comic.Title.EndsWith("Variant)");
        }
    }
}
