using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public interface INavigator
    {
        void NavigateToSeries(int id);
    }
}
