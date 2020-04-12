using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using ComicsLibrary.Common.Models;
using MarvelSharp.Model;

using Series = ComicsLibrary.Common.Models.Series;
using MarvelSeries = MarvelSharp.Model.Series;

using Comic = MarvelSharp.Model.Comic;

using ComicsLibrary.Common.Api;
using ComicsLibrary.Common;

namespace ComicsLibrary.MarvelUnlimited
{
    public class MappingProfile : Profile
    {
        private const string NoImagePlaceholder = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available.jpg";
        private const string ReaderLinkUrlBase = "http://marvel.com/digitalcomics/view.htm?";
        private const string ReadLinkFormat = "https://read.marvel.com/#/book/{0}";

        public MappingProfile()
        {
            CreateMap<Response<List<MarvelSeries>>, ApiResult<Series>>()
                .ForMember(dest => dest.Success, act => act.MapFrom(src => src.Success))
                .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Data.Total))
                .ForMember(dest => dest.Results, act => act.MapFrom(src => src.Data.Result));

            CreateMap<Response<List<Comic>>, ApiResult<Book>>()
                .ForMember(dest => dest.Success, act => act.MapFrom(src => src.Success))
                .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Data.Total))
                .ForMember(dest => dest.Results, act => act.MapFrom(src => src.Data.Result));

            CreateMap<MarvelSeries, Series>()
                .ForMember(dest => dest.ImageUrl, act => act.MapFrom(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.SourceItemID, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => GetTitleCase(src.Type)))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Url, act => act.Ignore())
                .ForMember(dest => dest.Books, act => act.Ignore());

            CreateMap<Comic, Book>()
                .ForMember(dest => dest.ImageUrl, act => act.MapFrom(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.ReadUrl, act => act.MapFrom(src => GetReaderUrl(src)))
                .ForMember(dest => dest.SourceItemID, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Creators, act => act.MapFrom(src => GetCreators(src)))
                .ForMember(dest => dest.OnSaleDate, act => act.MapFrom(src => GetOnSaleDate(src)))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.SeriesId, act => act.Ignore())
                .ForMember(dest => dest.Series, act => act.Ignore());

            CreateMap<MarvelSeries, SeriesUpdate>()
                .ForMember(dest => dest.ImageUrl, act => act.MapFrom(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.Url, act => act.MapFrom(src => src.Urls.FirstOrDefault().Value))
                .ForMember(dest => dest.Books, act => act.Ignore());

            CreateMap<Comic, BookUpdate>()
                .ForMember(dest => dest.BookTypeName, act => act.MapFrom(src => "Issues"))
                .ForMember(dest => dest.SourceItemID, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Number, act => act.MapFrom(src => src.IssueNumber))
                .ForMember(dest => dest.ImageUrl, act => act.MapFrom(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.OnSaleDate, act => act.MapFrom(src => GetOnSaleDate(src)))
                .ForMember(dest => dest.ReadUrl, act => act.MapFrom(src => GetReaderUrl(src)));
        }

        private static string GetCreators(Comic comic)
        {
            var roles = new[] {"writer", "artist"};

            return string.Join(", ", comic.Creators.Items.Where(i => roles.Contains(i.Role.ToLower())).Select(i => i.Name));
        }

        private static DateTimeOffset? GetOnSaleDate(Comic comic)
        {
            return comic.Dates.Where(d => d.Type.ToLower() == "onsaledate").Select(d => d.Date).Min();
        }

        private static string MapImageToString(Image image)
        {
            if (image == null || string.IsNullOrEmpty(image.Path) || string.IsNullOrEmpty(image.Extension))
                return string.Empty;

            var url = $"{image.Path}.{image.Extension}";

            if (url == NoImagePlaceholder)
                return string.Empty;

            return url;
        }

        private static string GetReaderUrl(MarvelSharp.Model.Comic result)
        {
            var readerLink = result.Urls.SingleOrDefault(u => u.Type == "reader")?.Value;
            if (readerLink == null)
            {
                return string.Empty;
            }

            var readerId = 0;

            foreach (var keyValuePair in readerLink.Replace(ReaderLinkUrlBase, "")
                .Split('&'))
            {
                var splitPair = keyValuePair.Split('=');
                if (splitPair[0] == "iid")
                {
                    readerId = int.Parse(splitPair[1]);
                }
            }

            if (readerId == 0)
            {
                return string.Empty;
            }

            return string.Format(ReadLinkFormat, readerId);
        }

        private static string GetTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }
    }
}
