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
                var series = uow.Repository<Series>().GetById(id);
                series.Abandoned = true;
                uow.Save();
            }
        }

        public void Reinstate(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>().GetById(id);
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

        public SeriesBookLists GetById(int seriesId, int numberOfComics)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var dbSeries = uow.Repository<Series>()
                    .Including(s => s.Books)
                    .Single(c => c.Id == seriesId);

                var series = new SeriesBookLists
                {
                    Id = dbSeries.Id,
                    Title = dbSeries.Title
                };

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
    }
}
