using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Series = ComicsLibrary.Blazor.Model.Series;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class SeriesCardBase : ComponentBase
    {
        [Parameter]
        public Series Series { get; set; }

        //[Parameter]
        //public Action<Model.Series, Shelf> MoveToShelf { get; set; }

        [Parameter]
        public List<SeriesAction> Actions { get; set; }

        //public List<SeriesAction> Actions { get; set; } = new List<SeriesAction>();

        //protected override void OnParametersSet()
        //{
        //    if (Series.Shelf == Shelf.Archived)
        //    {
        //        Actions.Add(new SeriesAction { Caption = "Unarchive", Icon = Icons.Material.Filled.Unarchive, ClickAction = Unarchive });
        //        Actions.Add(new SeriesAction { Caption = "Delete", Icon = Icons.Material.Filled.DeleteForever, ClickAction = Delete });
        //    }
        //    else
        //    {
        //        if (Series.Shelf == Shelf.Reading)
        //        {
        //            Actions.Add(new SeriesAction { Caption = "Put Aside", Icon = Icons.Material.Filled.Pause, ClickAction = PutAside });
        //        }
        //        else
        //        {
        //            Actions.Add(new SeriesAction { Caption = "Read Now", Icon = Icons.Material.Filled.PlayArrow, ClickAction = ReadNow });
        //        }

        //        if (Series.Shelf == Shelf.ToReadNext)
        //        {
        //            Actions.Add(new SeriesAction { Caption = "Remove from Read Next", Icon = Icons.Material.Filled.RemoveFromQueue, ClickAction = RemoveFromReadNext });
        //        }

        //        if (Series.Shelf != Shelf.Reading && Series.Shelf != Shelf.ToReadNext)
        //        {
        //            Actions.Add(new SeriesAction { Caption = "Read Next", Icon = Icons.Material.Filled.PlaylistAdd, ClickAction = AddToReadNext });
        //        }

        //        Actions.Add(new SeriesAction { Caption = "Archive", Icon = Icons.Material.Filled.Archive, ClickAction = Archive });
        //    }
        //}

        
    }
}
