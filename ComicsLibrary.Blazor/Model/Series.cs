using ComicsLibrary.Common;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Data;

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

            Actions = new List<SeriesAction>();

            Actions.Add(new SeriesAction { Caption = "View Series", Icon = Icons.Material.Filled.LibraryBooks });

            if (Shelf == Shelf.Archived)
            {
                Actions.Add(new SeriesAction { Caption = "Unarchive", Icon = Icons.Material.Filled.Unarchive });
                Actions.Add(new SeriesAction { Caption = "Delete", Icon = Icons.Material.Filled.DeleteForever });
            }
            else
            {
                if (Shelf == Shelf.Reading)
                {
                    Actions.Add(new SeriesAction { Caption = "Put Aside", Icon = Icons.Material.Filled.Pause });
                }
                else
                {
                    Actions.Add(new SeriesAction { Caption = "Read Now", Icon = Icons.Material.Filled.PlayArrow });
                }

                if (Shelf == Shelf.ToReadNext)
                {
                    Actions.Add(new SeriesAction { Caption = "Remove from Read Next", Icon = Icons.Material.Filled.RemoveFromQueue });
                }
                
                if (Shelf != Shelf.Reading && Shelf != Shelf.ToReadNext)
                {
                    Actions.Add(new SeriesAction { Caption = "Read Next", Icon = Icons.Material.Filled.PlaylistAdd });
                }

                Actions.Add(new SeriesAction { Caption = "Archive", Icon = Icons.Material.Filled.Archive });
            }
        }



        public string Years { get; set; }
        public bool Visible { get; set; } = true;

        public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();
    }
}
