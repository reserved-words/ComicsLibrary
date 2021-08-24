using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor
{
    public class Navigator : INavigator
    {
        private readonly NavigationManager _navigationManager;

        public Navigator(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigateToSeries(int id)
        {
            NavigateTo($"/series/{id}");
        }

        private void NavigateTo(string url)
        {
            _navigationManager.NavigateTo(url);
        }
    }
}
