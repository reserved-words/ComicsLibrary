using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;

using Comic = MarvelSharp.Model.Comic;
using Series = MarvelSharp.Model.Series;

namespace ComicsLibrary.MarvelUnlimited
{
    public interface IMapper
    {
        Book Map(Comic source);
        BookUpdate MapToUpdate(Comic source);
        SeriesUpdate Map(Series source);
    }
}