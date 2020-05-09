using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Common
{
    public class SeriesUpdater : ISeriesUpdater
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        private Dictionary<string, int> _bookTypes;

        public SeriesUpdater(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void UpdateSeries(int id, DateTime updateTime, SeriesUpdate updatedSeries)
        {
            using (var uow = _unitOfWorkFactory())
            {
                _bookTypes = uow.Repository<BookType>().ToDictionary(bt => bt.Name, bt => bt.ID);

                var oldSeries = uow.Repository<Series>().Single(s => s.Id == id);

                oldSeries.Title = GetNewValue(oldSeries.Title, updatedSeries.Title);
                oldSeries.StartYear = updatedSeries.StartYear;
                oldSeries.EndYear = updatedSeries.EndYear;
                oldSeries.Url = GetNewValue(oldSeries.Url, updatedSeries.Url);
                oldSeries.ImageUrl = GetNewValue(oldSeries.ImageUrl, updatedSeries.ImageUrl);
                oldSeries.LastUpdated = updateTime;

                var oldBooks = uow.Repository<Book>().Where(b => b.SeriesId == id).ToList();

                foreach (var book in updatedSeries.Books)
                {
                    TryUpdateBook(uow, id, oldBooks, book);
                }

                var homeBookTypes = uow.Repository<HomeBookType>()
                    .Where(h => h.SeriesId == id);

                foreach (var type in _bookTypes.Values)
                {
                    if (!homeBookTypes.Any(t => t.BookTypeId == type))
                    {
                        var hbt = new HomeBookType
                        {
                            SeriesId = id,
                            BookTypeId = type,
                            Enabled = true
                        };
                        uow.Repository<HomeBookType>().Insert(hbt);
                    }
                }

                uow.Save();
            }
        }

        private void TryUpdateBook(IUnitOfWork uow, int seriesID, List<Book> oldBooks, BookUpdate newBook)
        {
            var oldBook = oldBooks.SingleOrDefault(c => c.SourceItemID == newBook.SourceItemID);

            if (oldBook == null)
            {
                AddNewBook(uow, seriesID, newBook);
            }
            else
            {
                UpdateBook(oldBook, newBook);
            }
        }

        private void AddNewBook(IUnitOfWork uow, int seriesID, BookUpdate newBook)
        {
            var book = new Book
            {
                SeriesId = seriesID,
                SourceItemID = newBook.SourceItemID,
                DateAdded = DateTime.Now
            };

            UpdateBook(book, newBook);

            uow.Repository<Book>().Insert(book);
        }

        private void UpdateBook(Book oldBook, BookUpdate newBook)
        {
            oldBook.BookTypeID = _bookTypes[newBook.BookTypeName];
            oldBook.Title = GetNewValue(oldBook.Title, newBook.Title);
            oldBook.Number = newBook.Number ?? oldBook.Number;
            oldBook.Creators = GetNewValue(oldBook.Creators, newBook.Creators);
            oldBook.OnSaleDate = newBook.OnSaleDate ?? oldBook.OnSaleDate;
            oldBook.ImageUrl = GetNewValue(oldBook.ImageUrl, newBook.ImageUrl);
            oldBook.ReadUrl = GetNewValue(oldBook.ReadUrl, newBook.ReadUrl);
            oldBook.ReadUrlAdded = GetDateReadUrlAdded(oldBook, newBook);
        }

        private static DateTime? GetDateReadUrlAdded(Book oldBook, BookUpdate newBook)
        {
            if (oldBook.ReadUrlAdded.HasValue)
                return oldBook.ReadUrlAdded;

            if (!string.IsNullOrEmpty(newBook.ReadUrl))
                return DateTime.Now;

            return null;
        }

        private string GetNewValue(string oldValue, string newValue)
        {
            return string.IsNullOrEmpty(newValue) ? oldValue : newValue;
        }
    }
}
