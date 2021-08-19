using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILibrary = ComicsLibrary.Blazor.Services.ILibrary;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockLibrary : ILibrary
    {
        public async Task<List<NextComicInSeries>> GetNextToRead()
        {
            var list = new List<NextComicInSeries>();

            var allSeries = await GetShelf((int)Shelf.Reading);

            foreach (var series in allSeries)
            {
                var books = MockData.AllBooks[series.Id];
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

        public async Task<List<Model.Series>> GetShelf(int shelfId)
        {
            return MockData.AllSeries
                .Where(s => s.Shelf == (Shelf)shelfId)
                .Select(s => new Model.Series(s))
                .ToList();
        }

        public async Task<bool> UpdateShelf(int seriesId, int shelfId)
        {
            var seriesToMove = MockData.AllSeries.Single(s => s.Id == seriesId);
            seriesToMove.Shelf = (Shelf)shelfId;
            if (seriesToMove.Shelf == Shelf.Finished)
            {
                seriesToMove.Progress = 100;
                MockData.AllBooks[seriesId]
                    .Where(m => !m.IsRead)
                    .ToList()
                    .ForEach(m =>
                    {
                        m.DateRead = DateTime.Now;
                        m.IsRead = true;
                    });
            }
            else if (seriesToMove.Shelf == Shelf.Unread)
            {
                seriesToMove.Progress = 0;
                MockData.AllBooks[seriesId]
                    .Where(m => m.IsRead)
                    .ToList()
                    .ForEach(m =>
                    {
                        m.DateRead = null;
                        m.IsRead = false;
                    });
            }

            return true;
        }
    }
}
