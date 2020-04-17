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
        private readonly ILogger _logger;

        public Service(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper, ILogger logger)
        {
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

        public void HideBook(int id, bool isHidden)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.Hidden = isHidden;
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

        public List<NextComicInSeries> GetAllNextIssues()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Book>()
                    .Including(c => c.Series.HomeBookTypes)
                    .Where(c => !c.Hidden
                        && !c.Series.Abandoned 
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
