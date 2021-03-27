﻿using ComicsLibrary.Blazor.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class LibraryBase : ComponentBase
    {
        private string _searchString;

        protected string SearchString 
        {
            get { return _searchString; }
            set 
            { 
                _searchString = value;
                OnFilter();
            } 
        }

        protected Series selectedItem = null;
        protected bool TableView = false;
        protected bool GridView = true;

        [Parameter]
        public List<Series> Items { get; set; }
        
        [Parameter]
        public string ShelfName { get; set; }

        protected void OnFilter()
        {
            Items.ForEach(i => i.Visible = FilterFunc(i));
        }

        protected bool FilterFunc(Series element)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;
            if (element.Publisher.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Title.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Number}".Contains(SearchString))
                return true;
            return false;
        }

        protected void EnableTableView()
        {
            TableView = true;
            GridView = false;
        }

        protected void EnableGridView()
        {
            TableView = false;
            GridView = true;
        }
    }
}
