using System;
using System.Collections.Generic;
using System.Linq;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibrary _library;

        public LibraryService(ILibrary library)
        {
            _library = library;
        }

        public NextComicInSeries GetNextUnread(int seriesId)
        {
            return _library.GetNextUnread(seriesId);
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            return _library.GetAllNextIssues();
        }

        public void UpdateHomeBookType(HomeBookType homeBookType)
        {
            _library.UpdateHomeBookType(homeBookType);
        }

        public int GetProgress(int seriesId)
        {
            return GetSeries(seriesId).Progress;
        }

        public List<LibraryShelf> GetShelves()
        {
            var shelves = _library.GetSeries()
                .GroupBy(s => s.Status)
                .ToDictionary(s => s.Key, s => s.ToList());

            return Enum.GetValues(typeof(SeriesStatus)).OfType<SeriesStatus>()
                .Select(status => new LibraryShelf
                {
                    StatusId = (int)status,
                    Status = status.ToString(),
                    Series = shelves.TryGetValue(status, out List<LibrarySeries> series)
                        ? series
                        : new List<LibrarySeries>()
                })
                .ToList();
        }

        public LibrarySeries GetSeries(int id)
        {
            return _library.GetSeries(id);
        }
    }
}
