using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using MudBlazor;

namespace ComicsLibrary.Blazor
{
    public static class ColorConverter
    {
        public static Color Convert(string color)
        {
            return color switch
            {
                "purple" => Color.Primary,
                "pink" => Color.Secondary,
                "turquoise" => Color.Tertiary,
                "blue" => Color.Info,
                "green" => Color.Success,
                "gold" => Color.Warning,
                "red" => Color.Error,
                _ => Color.Dark,
            };
        }

        public static Color MudColor(this Series series) => Convert(series.Color);
        public static Color MudColor(this NextComicInSeries book) => Convert(book.Color);
    }
}
