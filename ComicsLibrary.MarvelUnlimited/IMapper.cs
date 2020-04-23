using ComicsLibrary.Common;
using MarvelSharp.Model;

namespace ComicsLibrary.MarvelUnlimited
{
    public interface IMapper
    {
        BookUpdate Map(Comic source);
        SeriesUpdate Map(Series source);
    }
}