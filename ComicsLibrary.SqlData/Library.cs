using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.SqlData
{
    public class Library : ILibrary
    {
        private readonly IDatabase _db;

        public Library(IDatabase db)
        {
            _db = db;
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            return _db.Query<NextComicInSeries>("GetHomeBooks");
        }

        public NextComicInSeries GetNextUnread(int seriesId)
        {
            return _db.QuerySingle<NextComicInSeries>("GetHomeBooks", new 
            { 
                SeriesId = seriesId 
            });
        }

        public List<LibrarySeries> GetSeries(Shelf shelf)
        {
            return _db.Query<LibrarySeries>("GetShelf", new { Shelf = (int)shelf });
        }

        public List<LibrarySeries> GetSeries()
        {
            return _db.Query<LibrarySeries>("GetSeries");
        }

        public LibrarySeries GetSeries(int seriesId)
        {
            return _db.QuerySingle<LibrarySeries>("GetSeries", new 
            { 
                SeriesId = seriesId 
            });
        }

        public void UpdateHomeBookType(HomeBookType homeBookType)
        {
            _db.Execute("UpdateHomeBookType", new 
            { 
                SeriesId = homeBookType.SeriesId, 
                BookTypeId = homeBookType.BookTypeId, 
                Enabled = homeBookType.Enabled 
            });
        }
    }
}
