using System;

namespace ComicsLibrary.Common
{
    public interface ISeriesUpdater
    {
        void UpdateSeries(int id, DateTime updateTime, SeriesUpdate updatedSeries);
    }
}
