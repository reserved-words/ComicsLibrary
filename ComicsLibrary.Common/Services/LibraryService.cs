using System;
using System.Collections.Generic;
using System.Linq;
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
            using (var uow = _unitOfWorkFactory())
            {
                var parameterName = "SeriesID";
                var parameter = new SqlParameter(parameterName, seriesId);

                return uow.Repository<NextComicInSeries>()
                    .GetFromSql($"ComicsLibrary.GetHomeBooks @{parameterName}", parameter)
                    .ToList()
                    .Single();
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
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Series>()
                    .Including(s => s.Books, s => s.HomeBookTypes)
                    .Single(s => s.Id == seriesId)
                    .GetProgress();
            }
        }
    }
}
