using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Services
{
    public interface ISeriesRepository
    {
        Task<SeriesDetail> GetSeries(int id);
        Task<List<Series>> GetShelf(Shelf? shelf, bool refreshCache);
        Task<bool> UpdateShelf(Series series, Shelf newShelf);
    }
}
