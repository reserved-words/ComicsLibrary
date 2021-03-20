﻿using ComicsLibrary.Common.Data;
using ComicsLibrary.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Updater
{
    public class Service
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly Func<int, ISourceUpdateService> _serviceFactory;
        private readonly ILogger _logger;
        private readonly IAsyncHelper _asyncHelper;

        private readonly ISeriesUpdater _updater;

        public Service(Func<int, ISourceUpdateService> serviceFactory, Func<IUnitOfWork> unitOfWorkFactory, ILogger logger,
            IAsyncHelper asyncHelper, ISeriesUpdater updater)
        {
            _serviceFactory = serviceFactory;
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _asyncHelper = asyncHelper;
            _updater = updater;
        }

        public async Task UpdateSeries(int maxNumber)
        {
            var seriesToUpdate = GetSeriesToUpdate(maxNumber);

            foreach (var series in seriesToUpdate)
            {
                try
                {
                    await TryLog(new Exception($"Updating series {series.Id}"), 1);

                    await UpdateSeries(series);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Series ID", series.Id);
                    ex.Data.Add("Source ID", series.SourceId);
                    ex.Data.Add("Source Item ID", series.SourceItemID);
                    await TryLog(ex, 3);
                }
            }
        }

        private List<Series> GetSeriesToUpdate(int maxNumber)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var twoYearsAgo = DateTime.Now.AddYears(-2).Year;
                var weekAgo = DateTime.Now.AddDays(-7);
                var yearAgo = DateTime.Now.AddYears(-1);

                return uow.Repository<Series>()
                    .Where(s => s.SourceId.HasValue
                        && s.SourceItemID.HasValue
                        && s.LastUpdated < weekAgo
                        && (!s.EndYear.HasValue || s.EndYear == 0 || s.EndYear > twoYearsAgo)
                        && s.Books.Any(c => c.DateAdded > yearAgo || c.ReadUrlAdded > yearAgo))
                    .OrderBy(s => s.LastUpdated)
                    .Take(maxNumber)
                    .ToList();
            }
        }

        private async Task UpdateSeries(Series series)
        {
            try
            {
                var updateTime = DateTime.Now;

                var sourceService = _serviceFactory(series.SourceId.Value);

                var updatedSeries = _asyncHelper.RunSync(() => sourceService.GetUpdatedSeries(series.SourceItemID.Value, series.Url, true));

                _updater.UpdateSeries(series.Id, updateTime, updatedSeries);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", series.Id);
                ex.Data.Add("Source ID", series.SourceItemID);
                ex.Data.Add("Source Item ID", series.SourceItemID);
                await TryLog(ex, 3);
            }
        }

        private async Task TryLog(Exception ex, int level)
        {
            try
            {
                await _logger.Log(ex, level);
            }
            catch { }
        }
    }
}
