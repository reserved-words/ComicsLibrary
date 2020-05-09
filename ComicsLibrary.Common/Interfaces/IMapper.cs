using ComicsLibrary.Common.Data;

namespace ComicsLibrary.Common
{
    public interface IMapper
    {
        LibrarySeries Map(Series source);
        Comic Map(Book source);
        Comic Map(SeriesBook source);
    }
}
