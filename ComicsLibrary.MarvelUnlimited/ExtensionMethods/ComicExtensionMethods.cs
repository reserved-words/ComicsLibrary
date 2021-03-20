﻿using MarvelSharp.Model;
using System;
using System.Linq;
using System.Web;

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

            var uri = new Uri(readerLink);
            var parameters = HttpUtility.ParseQueryString(uri.Query);
            var readerId = int.Parse(parameters.Get("iid"));

            if (readerId == 0)
            {
                return string.Empty;
            }

            return string.Format(Url.ReadLinkFormat, readerId);
        }
    }
}
