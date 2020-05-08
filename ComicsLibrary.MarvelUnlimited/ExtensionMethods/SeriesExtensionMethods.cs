using System.Linq;

namespace ComicsLibrary.MarvelUnlimited
{
    public static class SeriesExtensionMethods
    {
        public static string GetUrl(this MarvelSharp.Model.Series series)
        {
            return series.Urls.FirstOrDefault()?.Value;
        }

        public static string GetImageUrl(this MarvelSharp.Model.Series series)
        {
            return series.Thumbnail.MapToString();
        }
    }
}
