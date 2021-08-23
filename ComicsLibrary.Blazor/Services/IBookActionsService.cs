using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Services
{
    public interface IBookActionsService
    {
        List<BookAction> GetActions(Comic book);
    }
}
