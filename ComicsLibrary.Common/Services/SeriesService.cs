using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Common.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IMapper _mapper;

        public SeriesService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }


        public void Archive(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Models.Series>().GetById(id);
                series.Abandoned = true;
                uow.Save();
            }
        }

        public void Reinstate(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Models.Series>().GetById(id);
                series.Abandoned = false;
                uow.Save();
            }
        }


        public void Remove(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comicsToDelete = uow.Repository<Book>().Where(c => c.SeriesId == id).Select(c => c.Id).ToList();

                foreach (var comic in comicsToDelete)
                {
                    uow.Repository<Book>().Delete(comic);
                }

                uow.Repository<Models.Series>().Delete(id);
                uow.Save();
            }
        }

        public Api.Series GetById(int seriesId, int numberOfComics)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var dbSeries = uow.Repository<Models.Series>()
                    .Including(s => s.Books)
                    .Single(c => c.Id == seriesId);

                var series = _mapper.Map(dbSeries);

                if (numberOfComics == 0)
                    return series;

                var dbBookTypes = uow.Repository<BookType>()
                    .Including(bt => bt.HomeBookTypes)
                    .Select(bt => new
                    {
                        ID = bt.ID,
                        Name = bt.Name,
                        Home = bt.HomeBookTypes.Any(t => t.SeriesId == seriesId && t.Enabled)
                    })
                    .ToDictionary(t => t.ID, t => new { Name = t.Name, Home = t.Home });

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
                            .Select(b => _mapper.Map(b))
                            .ToArray()
                    })
                    .ToArray();

                return series;
            }
        }

        public List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Book>()
                    .Where(s => s.SeriesId == seriesId && s.BookTypeID == typeId)
                    .OrderByDescending(c => c.Number)
                    .Skip(offset)
                    .Take(limit)
                    .Select(b => _mapper.Map(b))
                    .ToList();
            }
        }

        public List<Api.Series> GetByStatus(SeriesStatus status)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = status == SeriesStatus.Reading
                    ? GetSeriesInProgress(uow)
                    : status == SeriesStatus.ToRead
                    ? GetSeriesToRead(uow)
                    : status == SeriesStatus.Read
                    ? GetSeriesFinished(uow)
                    : GetSeriesArchived(uow);

                var list = series.OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map(c))
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

        private IEnumerable<Models.Series> GetSeriesInProgress(IUnitOfWork uow)
        {
            return uow.Repository<Models.Series>()
                .Including(s => s.Books, s => s.HomeBookTypes)
                .Where(s => !s.Abandoned
                    // Series has some readable and read books
                    && s.Books.Any(c => !c.Hidden && s.HomeBookTypes.Any(h => h.BookTypeId == c.BookTypeID && h.Enabled) && c.DateRead.HasValue)
                    // Series has some readable and unread books
                    && s.Books.Any(c => !c.Hidden && s.HomeBookTypes.Any(h => h.BookTypeId == c.BookTypeID && h.Enabled) && !c.DateRead.HasValue));
        }

        private IEnumerable<Models.Series> GetSeriesToRead(IUnitOfWork uow)
        {
            return uow.Repository<Models.Series>()
                .Including(s => s.Books, s => s.HomeBookTypes)
                .Where(s => !s.Abandoned
                    // Series has some readable books
                    && s.Books.Any(c => !c.Hidden && s.HomeBookTypes.Any(h => h.BookTypeId == c.BookTypeID && h.Enabled))
                    // All readable books are unread
                    && s.Books.All(c => c.Hidden || !s.HomeBookTypes.Any(h => h.BookTypeId == c.BookTypeID && h.Enabled) || !c.DateRead.HasValue));
        }

        private IEnumerable<Models.Series> GetSeriesFinished(IUnitOfWork uow)
        {
            return uow.Repository<Models.Series>()
                .Including(s => s.Books, s => s.HomeBookTypes)
                .Where(s => !s.Abandoned
                    // All readable books are read
                    && s.Books.All(c => c.Hidden || !s.HomeBookTypes.Any(h => h.BookTypeId == c.BookTypeID && h.Enabled) || c.DateRead.HasValue));
        }

        private IEnumerable<Models.Series> GetSeriesArchived(IUnitOfWork uow)
        {
            return uow.Repository<Models.Series>()
                .Including(s => s.Books, s => s.HomeBookTypes)
                .Where(s => s.Abandoned);
        }


    }
}
