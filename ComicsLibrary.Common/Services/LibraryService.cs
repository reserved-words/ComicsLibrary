using System;
using System.Collections.Generic;
using System.Linq;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using Microsoft.Data.SqlClient;

using Series = ComicsLibrary.Common.Models.Series;

namespace ComicsLibrary.Common.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly ILogger _logger;

        public LibraryService(Func<IUnitOfWork> unitOfWorkFactory, ILogger logger)
        {
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public NextComicInSeries GetNextUnread(int seriesId)
        {
            var parameterName = "SeriesID";
            var parameter = new SqlParameter(parameterName, seriesId);

            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<NextComicInSeries>()
                    .GetFromSql($"ComicsLibrary.GetHomeBooks @{parameterName}", parameter)
                    .ToList()
                    .SingleOrDefault();
            }
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<NextComicInSeries>()
                    .GetFromSql($"ComicsLibrary.GetHomeBooks")
                    .ToList();
            }
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

        public int GetProgress(int seriesId)
        {
            return GetSeries(seriesId).Progress;
        }

        public List<LibraryShelf> GetShelves()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var shelves = uow.Repository<LibrarySeries>()
                     .GetFromSql($"ComicsLibrary.GetSeries")
                     .ToList()
                     .GroupBy(s => s.Status)
                     .ToDictionary(s => s.Key, s => s.ToList());

                var list = new List<LibraryShelf>();

                foreach (var status in Enum.GetValues(typeof(SeriesStatus)).OfType<SeriesStatus>())
                {
                    list.Add(new LibraryShelf
                    {
                        StatusId = (int)status,
                        Status = status.ToString(),
                        Series = shelves.TryGetValue(status, out List<LibrarySeries> series)
                            ? series
                            : new List<LibrarySeries>()
                    });
                }

                return list;
            }
        }

        public LibrarySeries GetSeries(int id)
        {
            var parameterName = "SeriesID";
            var parameter = new SqlParameter(parameterName, id);

            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<LibrarySeries>()
                    .GetFromSql($"ComicsLibrary.GetSeries @{parameterName}", parameter)
                    .ToList()
                    .Single();
            }
        }
    }
}
