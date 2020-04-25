using ComicsLibrary.Common;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using System.Linq;
using ApiComic = ComicsLibrary.Common.Api.Comic;
using Series = ComicsLibrary.Common.Models.Series;

namespace ComicsLibrary.Services.Mapper
{
    public class Mapper : IMapper
    {
        public LibrarySeries Map(Series source)
        {
            return new LibrarySeries
            {
                Id = source.Id,
                Title = source.Title,
                ImageUrl = source.GetImageUrl(),
                TotalBooks = source.Books.Count,
                UnreadBooks = source.GetValidBooks().Count(b => !b.DateRead.HasValue), 
                Archived = source.Abandoned,
            };
        }

        public ApiComic Map(Book source)
        {
            return new ApiComic
            {
                Id = source.Id,
                Title = source.Title,
                SeriesId = source.SeriesId,
                SeriesTitle = source.Series?.Title,
                IssueTitle = source.GetBookTitle(),
                SourceID = source.Series?.SourceId ?? 0,
                SourceItemID = source.SourceItemID,
                SourceName = source.Series?.Source?.Name,
                TypeID = source.BookType?.ID ?? 0,
                TypeName = source.BookType?.Name,
                IsRead = source.DateRead.HasValue,
                OnSaleDate = source.OnSaleDate.HasValue
                    ? source.OnSaleDate.Value.Date.ToShortDateString()
                    : "",
                DateAdded = source.DateAdded,
                ReadUrlAdded = source.ReadUrlAdded,
                ImageUrl = source.ImageUrl,
                ReadUrl = source.ReadUrl,
                IssueNumber = source.Number,
                DateRead = source.DateRead,
                Creators = source.Creators,
                Hidden = source.Hidden
            };
        }
    }
}
