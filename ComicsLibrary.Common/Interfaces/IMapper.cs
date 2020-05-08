using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IMapper
    {
        LibrarySeries Map(Series source);
        Comic Map(Book source);
        Comic Map(SeriesBook source);
    }
}
