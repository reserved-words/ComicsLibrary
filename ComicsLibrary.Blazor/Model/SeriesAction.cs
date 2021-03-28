using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Model
{
    public class SeriesAction
    {
        public string Caption { get; set; }
        public string Icon { get; set; }
        public Action<MouseEventArgs> ClickAction { get; set; }
        public Shelf MoveToShelf { get; set; }
    }
}
