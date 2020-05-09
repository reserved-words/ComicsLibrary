using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Common
{
    public interface ISeriesRepository
    {
        void Archive(int id);
        List<Book> GetBooks(int seriesId, int typeId, int limit, int offset);
        SeriesWithBooks GetSeriesWithBooks(int seriesId);
        void Reinstate(int id);
        void Remove(int id);
        int Insert(Series series);
        List<SeriesIds> GetAllIds(int sourceId);
    }
}