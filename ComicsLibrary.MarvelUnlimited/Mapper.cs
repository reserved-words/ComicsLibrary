using System;
using System.Linq;
using ComicsLibrary.Common;
using MarvelSharp.Model;

namespace ComicsLibrary.MarvelUnlimited
{
    public class Mapper : IMapper
    {
        private const string NoImagePlaceholder = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available.jpg";
        private const string ReaderLinkUrlBase = "http://marvel.com/digitalcomics/view.htm?";
        private const string ReadLinkFormat = "https://read.marvel.com/#/book/{0}";

        public SeriesUpdate Map(Series source)
        {
            return new SeriesUpdate
            {
                Title = source.Title,
                StartYear = source.StartYear,
                EndYear = source.EndYear,
                Url = source.Urls.FirstOrDefault()?.Value,
                ImageUrl = MapImageToString(source.Thumbnail)
            };
        }


        public BookUpdate Map(Comic source)
        {
            return new BookUpdate
            {
                ImageUrl = MapImageToString(source.Thumbnail),
                ReadUrl = GetReaderUrl(source),
                SourceItemID = source.Id,
                Creators = GetCreators(source),
                OnSaleDate = GetOnSaleDate(source),
                Title = source.Title,
                Number = source.IssueNumber,
                BookTypeName = "Issues",

            };
        }

        private static string GetCreators(Comic comic)
        {
            var roles = new[] { "writer", "artist" };

            return string.Join(", ", comic.Creators.Items.Where(i => roles.Contains(i.Role.ToLower())).Select(i => i.Name));
        }

        private static DateTimeOffset? GetOnSaleDate(Comic comic)
        {
            return comic.Dates.Where(d => d.Type.ToLower() == "onsaledate").Select(d => d.Date).Min();
        }

        private static string MapImageToString(MarvelSharp.Model.Image image)
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
