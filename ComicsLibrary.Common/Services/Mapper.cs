using ComicsLibrary.Common;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using System.Linq;
using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;
using Series = ComicsLibrary.Common.Models.Series;

namespace ComicsLibrary.Services.Mapper
{
    public class Mapper : IMapper
    {
        public ApiSeries Map(Series source)
        {
            return new ApiSeries
            {
                Id = source.Id,
                Title = source.Title,
                ImageUrl = source.GetImageUrl(),
                MainTitle = GetTitle(source, false),
                SubTitle = GetTitle(source, true),
                YearsActive = source.YearsActive(),
                Progress = source.GetProgress(),
                TotalComics = source.Books.Count,
                SourceID = source.Source?.ID ?? 0,
                SourceName = source.Source?.Name,
                UnreadIssues = source.GetValidBooks().Count(b => !b.DateRead.HasValue), 
                StartYear = source.StartYear,
                EndYear = source.EndYear,
                Type = source.Type,
                LastUpdated = source.LastUpdated,
                Abandoned = source.Abandoned,
                SourceItemID = source.SourceItemID.Value,
                Url = source.Url
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

        private static string GetTitle(Series series, bool subtitle)
        {
            if (series.Title == null)
                return null;

            var parenthesesStartAt = series.Title.IndexOf("(");
            if (parenthesesStartAt <= 1)
            {
                return subtitle ? "" : series.Title;
            }

            return subtitle
                ? series.Title.Substring(parenthesesStartAt)
                : series.Title.Substring(0, parenthesesStartAt - 1);
        }
    }
}
