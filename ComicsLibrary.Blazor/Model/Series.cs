using ComicsLibrary.Common;

namespace ComicsLibrary.Blazor.Model
{
    public class Series : LibrarySeries
    {
        public Series(LibrarySeries series)
        {
            Id = series.Id;
            ImageUrl = series.ImageUrl;
            Archived = series.Archived;
            Progress = series.Progress;
            Publisher = series.Publisher;
            PublisherIcon = series.PublisherIcon;
            Color = series.Color;
            Shelf = series.Shelf;

            (var title, var years) = series.SplitSeriesTitle();

            Title = title;
            Years = years;
        }



        public string Years { get; set; }
        public bool Visible { get; set; } = true;
    }
}
