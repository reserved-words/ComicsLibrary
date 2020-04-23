using ComicsLibrary.Common.Models;
using System.Collections.Generic;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;
using Series = ComicsLibrary.Common.Models.Series;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IMapper
    {
        ApiSeries Map(Series source);
        ApiComic Map(Book source);
    }
}
