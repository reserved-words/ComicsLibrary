﻿using ComicsLibrary.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class ReadingRepository : IReadingRepository
    {
        private readonly ILibrary _library;

        private List<NextComicInSeries> _cache = null;

        public ReadingRepository(ILibrary library)
        {
            _library = library;
        }

        public async Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache)
        {
            if (refreshCache)
            {
                _cache = null;
            }

            if (_cache == null)
            {
                _cache = await _library.GetNextToRead();
            }

            return _cache;
        }
    }
}
