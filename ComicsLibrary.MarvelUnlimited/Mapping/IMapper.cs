using ComicsLibrary.Common;
using ComicsLibrary.Common.Models;
using MarvelSharp.Model;

namespace ComicsLibrary.MarvelUnlimited
{
    public interface IMapper
    {
        Book Map(Comic source);
        BookUpdate MapToUpdate(Comic source);
        SeriesUpdate Map(MarvelSharp.Model.Series source);
    }
}