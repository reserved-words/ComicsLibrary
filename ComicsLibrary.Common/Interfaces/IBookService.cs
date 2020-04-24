using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;

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
