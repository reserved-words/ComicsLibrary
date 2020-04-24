using System.Collections.Generic;
using ComicsLibrary.Common.Api;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IBookService
    {
        void Hide(int id);
        void MarkAsRead(int id);
        void MarkAsUnread(int id);
        void Unhide(int id);
    }
}
