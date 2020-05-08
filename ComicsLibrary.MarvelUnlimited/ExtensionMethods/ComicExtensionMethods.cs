using MarvelSharp.Model;
using System;
using System.Linq;

namespace ComicsLibrary.MarvelUnlimited
{
    public static class ComicExtensionMethods
    {
        public static string GetCreators(this Comic comic)
        {
            var roles = new[] { "writer", "artist" };
            
            var creatorNames = comic.Creators.Items
                .Where(i => roles.Contains(i.Role.ToLower()))
                .Select(c => c.Name);

            return string.Join(", ", creatorNames);
        }

        public static DateTimeOffset? GetOnSaleDate(this Comic comic)
        {
            return comic.Dates.Where(d => d.Type.ToLower() == "onsaledate").Select(d => d.Date).Min();
        }

        public static string GetImageUrl(this Comic comic)
        {
            return comic.Thumbnail.MapToString();
        }

        public static string GetReaderUrl(this Comic comic)
        {
            var readerLink = comic.Urls.SingleOrDefault(u => u.Type == "reader")?.Value;
            if (readerLink == null)
            {
                return string.Empty;
            }

            var readerId = 0;

            foreach (var keyValuePair in readerLink.Replace(Url.ReaderLinkUrlBase, "")
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

            return string.Format(Url.ReadLinkFormat, readerId);
        }
    }
}
