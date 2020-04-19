using System;
using System.Linq;
using AutoMapper;
using ComicsLibrary.Common.Models;

using Series = ComicsLibrary.Common.Models.Series;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;

using NextComicInSeries = ComicsLibrary.Common.NextComicInSeries;
using ComicsLibrary.Common;

namespace ComicsLibrary.Services.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Book, ApiComic>()
                .ForMember(s => s.SeriesId, act => act.MapFrom(src => src.SeriesId))
                .ForMember(s => s.SeriesTitle, act => act.MapFrom(src => src.Series.Title))
                .ForMember(s => s.IssueTitle, act => act.MapFrom(src => GetBookTitle(src)))
                .ForMember(s => s.SourceID, act => act.MapFrom(src => src.Series.Source.ID))
                .ForMember(s => s.SourceName, act => act.MapFrom(src => src.Series.Source.Name))
                .ForMember(s => s.TypeID, act => act.MapFrom(src => src.BookType.ID))
                .ForMember(s => s.TypeName, act => act.MapFrom(src => src.BookType.Name))
                .ForMember(s => s.IsRead, act => act.MapFrom(src => src.DateRead.HasValue))
                .ForMember(s => s.OnSaleDate, act => act.MapFrom(src => src.OnSaleDate.HasValue ? src.OnSaleDate.Value.Date.ToShortDateString() : ""));

            CreateMap<Series, ApiSeries>()
                .ForMember(s => s.ImageUrl, act => act.MapFrom(src => GetImageUrl(src)))
                .ForMember(s => s.MainTitle, act => act.MapFrom(src => GetTitle(src, false)))
                .ForMember(s => s.SubTitle, act => act.MapFrom(src => GetTitle(src, true)))
                .ForMember(s => s.YearsActive, act => act.MapFrom(src => GetYearsActive(src)))
                .ForMember(s => s.Progress, act => act.MapFrom(src => src.GetProgress()))
                .ForMember(s => s.TotalComics, act => act.MapFrom(src => src.Books.Count))
                .ForMember(s => s.SourceID, act => act.MapFrom(src => src.Source.ID))
                .ForMember(s => s.SourceName, act => act.MapFrom(src => src.Source.Name))
                .ForMember(s => s.UnreadIssues, act => act.MapFrom(src => 
                    src.Books.Count(c => !c.DateRead.HasValue)));

            CreateMap<ApiSeries, Series>()
                .ForMember(s => s.ImageUrl, act => act.MapFrom(src => src.ImageUrl))
                .ForMember(s => s.Title, act => act.MapFrom(src => src.Title))
                .ForMember(s => s.StartYear, act => act.MapFrom(src => src.StartYear))
                .ForMember(s => s.EndYear, act => act.MapFrom(src => src.EndYear))
                .ForMember(s => s.Type, act => act.MapFrom(src => src.Type))
                .ForMember(s => s.SourceItemID, act => act.MapFrom(src => src.SourceItemID))
                .ForMember(s => s.Url, act => act.MapFrom(src => src.Url))
                .ForAllOtherMembers(s => s.Ignore());

        CreateMap<Book, Book>()
                .ForMember(src => src.Title, act => act.MapFrom(src => src.Title))
                .ForMember(src => src.ImageUrl, act => act.MapFrom(src => src.ImageUrl))
                .ForMember(src => src.ReadUrl, act => act.MapFrom(src => src.ReadUrl))
                .ForMember(src => src.Number, act => act.MapFrom(src => src.Number))
                .ForMember(src => src.Creators, act => act.MapFrom(src => src.Creators))
                .ForMember(src => src.OnSaleDate, act => act.MapFrom(src => src.OnSaleDate))
                .ForAllOtherMembers(act => act.Ignore());
        }

        private string GetImageUrl(Series series)
        {
            var validTypes = series.HomeBookTypes
                .Where(bt => bt.Enabled)
                .Select(bt => bt.BookTypeId)
                .ToList();

            var validBooks = series.Books
                .Where(b => !b.Hidden
                    && b.BookTypeID.HasValue
                    && validTypes.Contains(b.BookTypeID.Value));

            return validBooks
                .OrderBy(b => b.Number)
                .First().ImageUrl;
        }

        private static string GetBookTitle(Book book)
        {
            return book.BookTypeID == 1
                ? $"#{book.Number}"
                : book.BookTypeID == 2
                ? $"Vol. {book.Number}"
                : "";
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

        private static string GetYearsActive(Series series)
        {
            if (series.StartYear == null)
                return series.EndYear?.ToString();

            if (series.EndYear == null || series.StartYear == series.EndYear)
                return series.StartYear.ToString();

            return $"{series.StartYear} - {series.EndYear}";
        }

        private static string GetSeriesTitle(Series series)
        {
            return series.StartYear == null
                ? series.MainTitle
                : $"{series.MainTitle} ({series.StartYear})";
        }
    }
}
