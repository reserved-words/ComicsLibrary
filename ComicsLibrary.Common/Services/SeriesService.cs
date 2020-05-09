using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Common
{
    public class SeriesService : ISeriesService
    {
        private readonly IMapper _mapper;
        private readonly ISeriesRepository _repository;

        public SeriesService(ISeriesRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public void Archive(int id)
        {
            _repository.Archive(id);
        }

        public void Reinstate(int id)
        {
            _repository.Reinstate(id);
        }


        public void Remove(int id)
        {
            _repository.Remove(id);
        }

        public SeriesBookLists GetById(int seriesId, int numberOfComics)
        {
            // TODO: Limit number of comics fetched
            // TODO: If number of comics = 0 just get series details

            var seriesWithBooks = _repository.GetSeriesWithBooks(seriesId);

            var series = new SeriesBookLists
            {
                Id = seriesWithBooks.Summary.Id,
                Title = seriesWithBooks.Summary.Title
            };

            if (numberOfComics == 0)
                return series;

            var bookTypes = seriesWithBooks.BookTypes
                .ToDictionary(t => t.Id, t => new { Name = t.Name, Enabled = t.Enabled });

            series.BookLists = seriesWithBooks.Books
                .GroupBy(b => b.BookTypeID)
                .Select(g => new BookList
                {
                    TypeId = g.Key,
                    TypeName = bookTypes[g.Key].Name,
                    TotalBooks = g.Count(),
                    Home = bookTypes[g.Key].Enabled,
                    Books = g.OrderByDescending(c => c.Number)
                        .Take(numberOfComics)
                        .Select(b => _mapper.Map(b))
                        .ToArray()
                })
                .ToArray();

            return series;
        }

        public List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            return _repository.GetBooks(seriesId, typeId, limit, offset)
                .Select(b => _mapper.Map(b))
                .ToList();
        }
    }
}
