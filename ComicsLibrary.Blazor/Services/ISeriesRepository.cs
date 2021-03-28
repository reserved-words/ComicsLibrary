using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public interface ISeriesRepository
    {
        Task<List<LibrarySeries>> GetShelf(Shelf shelf, bool refreshCache);
    }
}
