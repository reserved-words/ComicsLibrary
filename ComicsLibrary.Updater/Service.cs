using ComicsLibrary.Common.Models;
using ComicsLibrary.Common.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using ComicsLibrary.Common;

namespace ComicsLibrary.Updater
{
    public class Service
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly Func<int, ISourceUpdateService> _serviceFactory;
        private readonly ILogger _logger;
        private readonly IAsyncHelper _asyncHelper;

        private Dictionary<string, int> _bookTypes;

        public Service(Func<int, ISourceUpdateService> serviceFactory, Func<IUnitOfWork> unitOfWorkFactory, ILogger logger, IAsyncHelper asyncHelper)
        {
            _serviceFactory = serviceFactory;
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _asyncHelper = asyncHelper;
        }

        public void UpdateSeries(int maxNumber)
        {
            var seriesToUpdate = GetSeriesToUpdate(maxNumber);

            foreach (var series in seriesToUpdate)
            {
                try
                {
                    TryLog(new Exception($"Updating series {series.Id}"));

                    UpdateSeries(series);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Series ID", series.Id);
                    ex.Data.Add("Source ID", series.SourceId);
                    ex.Data.Add("Source Item ID", series.SourceItemID);
                    TryLog(ex);
                }
            }
        }

        private List<Series> GetSeriesToUpdate(int maxNumber)
        {
            using (var uow = _unitOfWorkFactory())
            {
                _bookTypes = uow.Repository<BookType>().ToDictionary(bt => bt.Name, bt => bt.ID);

                var twoYearsAgo = DateTime.Now.AddYears(-2).Year;
                var weekAgo = DateTime.Now.AddDays(-7);
                var yearAgo = DateTime.Now.AddYears(-1);

                return uow.Repository<Series>()
                    .Where(s => s.SourceId.HasValue
                        && s.SourceItemID.HasValue
                        && s.LastUpdated < weekAgo
                        && (!s.EndYear.HasValue || s.EndYear > twoYearsAgo)
                        && s.Comics.Any(c => c.DateAdded > yearAgo || c.ReadUrlAdded > yearAgo))
                    .OrderBy(s => s.LastUpdated)
                    .Take(maxNumber)
                    .ToList();
            }
        }

        private void UpdateSeries(Series series)
        {
            try
            {
                var updateTime = DateTime.Now;

                var sourceService = _serviceFactory(series.SourceId.Value);

                var newSeries = _asyncHelper.RunSync(() => sourceService.GetUpdatedSeries(series.SourceItemID.Value, series.Url));

                using (var uow = _unitOfWorkFactory())
                {
                    var oldSeries = uow.Repository<Series>().Single(s => s.Id == series.Id);

                    oldSeries.Title = newSeries.Title;
                    oldSeries.StartYear = newSeries.StartYear;
                    oldSeries.EndYear = newSeries.EndYear;
                    oldSeries.Url = newSeries.Url;
                    oldSeries.ImageUrl = newSeries.ImageUrl;
                    oldSeries.LastUpdated = updateTime;

                    var oldBooks = uow.Repository<Book>().Where(b => b.SeriesId == series.Id).ToList();

                    foreach (var book in newSeries.Books)
                    {
                        TryUpdateBook(uow, series.Id, oldBooks, book);
                    }

                    uow.Save();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", series.Id);
                ex.Data.Add("Source ID", series.SourceItemID);
                ex.Data.Add("Source Item ID", series.SourceItemID);
                TryLog(ex);
            }
        }

        private void TryUpdateBook(IUnitOfWork uow, int seriesID, List<Book> oldBooks, BookUpdate newBook)
        {
            try
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
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesID);
                ex.Data.Add("Comic Issue Number", newBook.Number.HasValue ? newBook.Number.ToString() : "null");
                ex.Data.Add("Comic Source Item ID", newBook.SourceItemID.HasValue ? newBook.SourceItemID.ToString() : "null");
                TryLog(ex);
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
            oldBook.Title = newBook.Title;
            oldBook.Number = newBook.Number;
            oldBook.Creators = newBook.Creators;
            oldBook.OnSaleDate = newBook.OnSaleDate;
            oldBook.ImageUrl = newBook.ImageUrl;
            oldBook.ReadUrl = newBook.ReadUrl;
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

        private void TryLog(Exception ex)
        {
            try
            {
                _logger.Log(ex);
            }
            catch { }
        }
    }
}
