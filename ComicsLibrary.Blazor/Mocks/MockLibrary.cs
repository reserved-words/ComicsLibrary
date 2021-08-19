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
                list.Add(GetFirstUnreadBook(series));
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

        public async Task<NextComicInSeries> MarkReadAndGetNext(int bookId)
        {
            var book = GetBook(bookId);

            UpdateReadStatus(book, true);

            return GetFirstUnreadBook(book.SeriesId);
        }

        public async Task<NextComicInSeries> MarkPreviousUnreadAndGet(int bookId)
        {
            var book = GetBook(bookId);

            var readBooks = MockData
                .AllBooks[book.SeriesId]
                .Where(b => b.IsRead)
                .OrderBy(b => b.Id);

            var lastRead = readBooks.Last();

            UpdateReadStatus(lastRead, false);

            var series = GetSeries(lastRead.SeriesId);

            return Map(lastRead, series);
        }



        private static NextComicInSeries GetFirstUnreadBook(Model.Series series)
        {
            var unreadBooks = MockData
                .AllBooks[series.Id]
                .Where(b => !b.IsRead)
                .OrderBy(b => b.Id);

            var firstUnread = unreadBooks.First();

            return Map(firstUnread, series);
        }

        private static NextComicInSeries Map(Comic book, Model.Series series)
        {
            var allBooks = MockData.AllBooks[series.Id];
            var total = allBooks.Count();
            var unread = allBooks.Count(b => !b.IsRead);
            var read = allBooks.Count(b => b.IsRead);

            return new NextComicInSeries
            {
                Id = book.Id,
                SeriesId = series.Id,
                SeriesTitle = series.Title,
                Years = series.Years,
                Publisher = series.Publisher,
                Color = series.Color,
                IssueTitle = book.IssueTitle,
                ImageUrl = book.ImageUrl,
                ReadUrl = book.ReadUrl,
                UnreadBooks = unread,
                Creators = "",
                Progress = GetProgress(read, total)
            };
        }

        private static int GetProgress(int read, int total)
        {
            var progress = 100 * (double)read / (double)total;
            return (int)Math.Round(progress, 0);
        }

        private static void UpdateReadStatus(Comic comic, bool read)
        {
            comic.IsRead = read;
            comic.DateRead = read ? DateTime.Now : (DateTime?)null;
        }

        private static NextComicInSeries GetFirstUnreadBook(int seriesId)
        { 
            var series = GetSeries(seriesId);

            return GetFirstUnreadBook(series);
        }

        private static Model.Series GetSeries(int seriesId)
        {
            return new Model.Series(MockData.GetSeries(seriesId));
        }

        private static Comic GetBook(int bookId)
        {
            return MockData.AllBooks
                .SelectMany(s => s.Value)
                .Single(c => c.Id == bookId);
        }
    }
}
