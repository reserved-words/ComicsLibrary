using ComicsLibrary.Blazor.Model;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Pages.Library
{
    public class ArchivedBase : ShelfBase
    {
        protected override async Task OnInitializedAsync()
        {
            Items = new List<Series>
            {
                new Series { Id = 1, Publisher = "DC", Title = "Batman", Number = 1, PublisherIcon = "DC", Years = "2019-", Color = Color.Info, Progress = 50, ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51Ibe1bTtdL._SX313_BO1,204,203,200_.jpg"  },
                new Series { Id = 2, Publisher = "DC", Title = "Superman", Number = 2, PublisherIcon = "DC", Years = "2015-2017", Color = Color.Info, Progress = 65, ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/519RbENDlTL._SX329_BO1,204,203,200_.jpg"  },
                new Series { Id = 3, Publisher = "Marvel", Title = "Iron Man", Number = 1, PublisherIcon = "M", Years = "2021-", Color = Color.Error, Progress = 20, ImageUrl = "https://i.pinimg.com/originals/3d/7e/ac/3d7eac416e5ced11f04fb7e96ddfe3a8.jpg"  },
                new Series { Id = 4, Publisher = "Marvel", Title = "Captain Marvel", Number = 2 , PublisherIcon = "M", Years = "2004-2006", Color = Color.Error, Progress = 87, ImageUrl = "https://s3.amazonaws.com/comicgeeks/comics/covers/large-9754559.jpg" },
                new Series { Id = 5, Publisher = "Image Comics", Title = "Sex Criminals", Number = 1, PublisherIcon = "I", Color = Color.Primary, Progress = 15, ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/71dyjDZWEML.jpg"  },
                new Series { Id = 5, Publisher = "Boom Studios", Title = "Lumberjanes", Number = 1, PublisherIcon = "B", Color = Color.Secondary, Progress = 76, ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/81U4rjx0Z0L.jpg"  }
            };
        }
    }
}
