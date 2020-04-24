using MarvelSharp.Model;

namespace ComicsLibrary.MarvelUnlimited
{
    public static class ImageExtensionMethods
    {
        public static string MapToString(this Image image)
        {
            if (image == null || string.IsNullOrEmpty(image.Path) || string.IsNullOrEmpty(image.Extension))
                return string.Empty;

            var url = $"{image.Path}.{image.Extension}";

            if (url == Url.NoImagePlaceholder)
                return string.Empty;

            return url;
        }
    }
}
