using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.SqlData
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly IDatabase _db;

        public SeriesRepository(IDatabase db)
        {
            _db = db;
        }


        public void Archive(int id)
        {
            _db.Execute("UpdateArchivedStatus", new { SeriesId = id, Archived = true });
        }

        public void Reinstate(int id)
        {
            _db.Execute("UpdateArchivedStatus", new { SeriesId = id, Archived = false });
        }

        public void Remove(int id)
        {
            _db.Execute("DeleteSeries", new { SeriesId = id });
        }

        public SeriesWithBooks GetSeriesWithBooks(int seriesId)
        {
            var series = new SeriesWithBooks();
            _db.Populate(series, "GetSeriesWithBooks", new { SeriesId = seriesId });
            return series;
        }

        public List<Book> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            return _db.Query<Book>("GetBooks", new { SeriesId = seriesId, TypeId = typeId, Limit = limit, Offset = offset });
        }

        public int Insert(Series series)
        {
            _db.Execute("InsertSeries", new {
                SourceId = series.SourceId,
                SourceItemID = series.SourceItemID,
                Title = series.Title,
                Url = series.Url,
                ImageUrl = series.ImageUrl
            }, out int id);

            return id;
        }

        public List<SeriesIds> GetAllIds(int sourceId)
        {
            return _db.Query<SeriesIds>("GetSeriesIds", new { SourceId = sourceId });
        }
    }
}
