using ComicsLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public interface IReadingRepository
    {
        Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache);
    }
}
