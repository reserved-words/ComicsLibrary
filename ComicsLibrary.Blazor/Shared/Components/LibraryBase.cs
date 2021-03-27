using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class LibraryBase : ComponentBase
    {
        protected string searchString = "";
        protected Series selectedItem = null;
        protected HashSet<Series> selectedItems = new HashSet<Series>();
        protected bool tableView = false;
        protected bool gridView = true;

        [Parameter]
        public IEnumerable<Series> Items { get; set; }
        
        [Parameter]
        public string ShelfName { get; set; }

        protected bool FilterFunc(Series element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Publisher.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Number}".Contains(searchString))
                return true;
            return false;
        }

        protected void EnableTableView()
        {
            tableView = true;
            gridView = false;
        }

        protected void EnableGridView()
        {
            tableView = false;
            gridView = true;
        }
    }
}
