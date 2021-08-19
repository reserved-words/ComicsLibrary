using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public interface ILibrary
    {
        Task<List<NextComicInSeries>> GetNextToRead();
        Task<List<Series>> GetShelf(int shelfId);
        Task<bool> UpdateShelf(int seriesId, int shelfId);
    }
}
