using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockReadingRepository : IReadingRepository
    {
        public async Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache)
        {
            var list = new List<NextComicInSeries>();

            var allSeries = MockSeriesRepository.GetShelf(Shelf.Reading);
                
            foreach (var series in allSeries)
            {
                var books = MockSeriesRepository.AllBooks[series.Id];
                var firstUnread = books.First(b => !b.IsRead);
                list.Add(new NextComicInSeries
                {
                    Id = firstUnread.Id,
                    SeriesId = series.Id,
                    SeriesTitle = series.Title,
                    Years = series.Years,
                    Publisher = series.Publisher,
                    Color = series.Color,
                    IssueTitle = firstUnread.IssueTitle,
                    ImageUrl = firstUnread.ImageUrl,
                    ReadUrl = firstUnread.ReadUrl,
                    UnreadBooks = books.Count(b => !b.IsRead),
                    Creators = "",
                    Progress = series.Progress
                });
            }

            return list;
        }
    }
}
