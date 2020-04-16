using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common.Interfaces
{
    public interface ISeriesUpdater
    {
        void UpdateSeries(int id, DateTime updateTime, SeriesUpdate updatedSeries);
    }
}
