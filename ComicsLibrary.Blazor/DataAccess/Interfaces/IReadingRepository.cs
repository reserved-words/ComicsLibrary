using ComicsLibrary.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor
{
    public interface IReadingRepository
    {
        Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache);
        Task<NextComicInSeries> MoveNext(NextComicInSeries current);
        Task<NextComicInSeries> MovePrevious(NextComicInSeries current);
    }
}
