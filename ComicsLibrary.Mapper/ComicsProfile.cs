using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using ComicsLibrary.Common.Models;
using MarvelSharp.Model;

using Comic = ComicsLibrary.Common.Models.Comic;
using Series = ComicsLibrary.Common.Models.Series;
using Character = ComicsLibrary.Common.Models.Character;

using MarvelComic = MarvelSharp.Model.Comic;
using MarvelSeries = MarvelSharp.Model.Series;
using MarvelCharacter = MarvelSharp.Model.Character;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;

namespace ComicsLibrary.Mapper
{
    public class ComicsProfile : Profile
    {
        private const string NoImagePlaceholder = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available.jpg";
        private const string ReaderLinkUrlBase = "http://marvel.com/digitalcomics/view.htm?";
        private const string ReadLinkFormat = "https://read.marvel.com/#/book/{0}";

        public ComicsProfile()
        {
            CreateMap<Response<List<MarvelSeries>>, ApiResult<Series>>()
                .ForMember(dest => dest.Success, act => act.MapFrom(src => src.Success))
                .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Data.Total))
                .ForMember(dest => dest.Results, act => act.MapFrom(src => src.Data.Result));

            CreateMap<Response<List<MarvelComic>>, ApiResult<Comic>>()
                .ForMember(dest => dest.Success, act => act.MapFrom(src => src.Success))
                .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Data.Total))
                .ForMember(dest => dest.Results, act => act.MapFrom(src => src.Data.Result));

            CreateMap<Response<List<MarvelCharacter>>, ApiResult<Character>>()
                .ForMember(dest => dest.Success, act => act.MapFrom(src => src.Success))
                .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Data.Total))
                .ForMember(dest => dest.Results, act => act.MapFrom(src => src.Data.Result));

            CreateMap<MarvelSeries, Series>()
                .ForMember(dest => dest.ImageUrl, act => act.ResolveUsing(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.MarvelId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Characters, act => act.MapFrom(src => string.Join(", ", src.Characters.Items.Select(i => i.Name))))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => GetTitleCase(src.Type)))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Order, act => act.Ignore())
                .ForMember(dest => dest.Url, act => act.Ignore())
                .ForMember(dest => dest.Comics, act => act.Ignore());

            CreateMap<MarvelComic, Comic>()
                .ForMember(dest => dest.ImageUrl, act => act.ResolveUsing(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.ReadUrl, act => act.ResolveUsing(GetReaderUrl))
                .ForMember(dest => dest.MarvelId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Creators, act => act.MapFrom(src => GetCreators(src)))
                .ForMember(dest => dest.OnSaleDate, act => act.ResolveUsing(GetOnSaleDate))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Url, act => act.Ignore())
                .ForMember(dest => dest.SeriesId, act => act.Ignore())
                .ForMember(dest => dest.Series, act => act.Ignore());

            CreateMap<MarvelCharacter, Character>()
                .ForMember(dest => dest.ImageUrl, act => act.ResolveUsing(src => MapImageToString(src.Thumbnail)))
                .ForMember(dest => dest.MarvelId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Url, act => act.Ignore());

            CreateMap<Comic, ApiComic>()
                .ForMember(s => s.SeriesId, act => act.MapFrom(src => src.SeriesId))
                .ForMember(s => s.SeriesTitle, act => act.MapFrom(src => src.Series.Title))
                .ForMember(s => s.IssueTitle, act => act.MapFrom(src => GetIssueTitle(src)));

            CreateMap<Series, ApiSeries>()
                .ForMember(s => s.ImageUrl, act => act.MapFrom(src => src.Comics.FirstOrDefault().ImageUrl))
                .ForMember(s => s.MainTitle, act => act.MapFrom(src => GetTitle(src, false)))
                .ForMember(s => s.SubTitle, act => act.MapFrom(src => GetTitle(src, true)))
                .ForMember(s => s.YearsActive, act => act.MapFrom(src => GetYearsActive(src)))
                .ForMember(s => s.Progress, act => act.MapFrom(src => GetProgress(src)))
                .ForMember(s => s.TotalComics, act => act.MapFrom(src => src.Comics.Count))
                .ForMember(s => s.UnreadAvailableComics, act => act.MapFrom(src => 
                    src.Comics.Count(c => !c.IsRead && !string.IsNullOrEmpty(c.ReadUrl))));

            CreateMap<ApiSeries, Series>()
                .ForMember(s => s.ImageUrl, act => act.MapFrom(src => src.ImageUrl))
                .ForMember(s => s.Title, act => act.MapFrom(src => src.Title))
                .ForMember(s => s.StartYear, act => act.MapFrom(src => src.StartYear))
                .ForMember(s => s.EndYear, act => act.MapFrom(src => src.EndYear))
                .ForMember(s => s.Type, act => act.MapFrom(src => src.Type))
                .ForMember(s => s.MarvelId, act => act.MapFrom(src => src.MarvelId))
                .ForMember(s => s.Url, act => act.MapFrom(src => src.Url))
                .ForAllOtherMembers(s => s.Ignore());

        CreateMap<Comic, Comic>()
                .ForMember(src => src.Title, act => act.MapFrom(src => src.Title))
                .ForMember(src => src.ImageUrl, act => act.MapFrom(src => src.ImageUrl))
                .ForMember(src => src.ReadUrl, act => act.MapFrom(src => src.ReadUrl))
                .ForMember(src => src.DiamondCode, act => act.MapFrom(src => src.DiamondCode))
                .ForMember(src => src.Isbn, act => act.MapFrom(src => src.Isbn))
                .ForMember(src => src.IssueNumber, act => act.MapFrom(src => src.IssueNumber))
                .ForMember(src => src.Upc, act => act.MapFrom(src => src.Upc))
                .ForMember(src => src.Url, act => act.MapFrom(src => src.Url))
                .ForMember(src => src.Creators, act => act.MapFrom(src => src.Creators))
                .ForMember(src => src.OnSaleDate, act => act.MapFrom(src => src.OnSaleDate))
                .ForAllOtherMembers(act => act.Ignore());
        }

        private static int GetProgress(Series series)
        {
            return (int)Math.Round(100 * (double)series.Comics.Count(c => c.IsRead) / series.Comics.Count());
        }

        private static string GetIssueTitle(Comic comic)
        {
            if (!comic.OnSaleDate.HasValue)
                return $"#{comic.IssueNumber}";

            return $"#{comic.IssueNumber} ({comic.OnSaleDate.Value.Date.ToString("dd/MM/yyyy")})";
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

        private static string GetTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        private static string GetCreators(MarvelSharp.Model.Comic comic)
        {
            var roles = new[] {"writer", "artist"};

            return string.Join(", ", comic.Creators.Items.Where(i => roles.Contains(i.Role.ToLower())).Select(i => i.Name));
        }

        private static DateTimeOffset? GetOnSaleDate(MarvelSharp.Model.Comic comic)
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
    }
}
