
namespace ComicsLibrary.Common
{
    public interface IBookRepository
    {
        void Hide(int id);
        void MarkAsRead(int id);
        void MarkAsUnread(int id);
        void Unhide(int id);
    }
}
