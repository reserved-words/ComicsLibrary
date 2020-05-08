using ComicsLibrary.Common;

namespace ComicsLibrary.SqlData
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabase _db;

        public BookRepository(IDatabase db)
        {
            _db = db;
        }

        public void Hide(int id)
        {
            _db.Execute("UpdateBookHideStatus", new { Id = id, Hide = true });
        }

        public void MarkAsRead(int id)
        {
            _db.Execute("UpdateBookReadStatus", new { Id = id, Read = true });
        }

        public void MarkAsUnread(int id)
        {
            _db.Execute("UpdateBookReadStatus", new { Id = id, Read = false });
        }

        public void Unhide(int id)
        {
            _db.Execute("UpdateBookHideStatus", new { Id = id, Hide = false });
        }
    }
}
