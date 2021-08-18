using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockSeriesRepository : Blazor.Services.ISeriesRepository
    {
        private static List<LibrarySeries> AllSeries = new List<LibrarySeries>
        {
            new LibrarySeries
            {
                Id = 1,
                Title = "Black Widow (2020 - Present)",
                ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/c/40/5f3d36dc73d2a.jpg",
                Progress = 45,
                Publisher = "Marvel",
                PublisherIcon = "M",
                Color = "red",
                Shelf = Shelf.Reading
            },
            new LibrarySeries
            {
                Id = 3,
                Title = "Harley Quinn (2016-)",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg",
                Progress = 0,
                Publisher = "DC Comics",
                PublisherIcon = "DC",
                Color = "blue",
                Shelf = Shelf.ToReadNext
            },
            new LibrarySeries
            {
                Id = 4,
                Title = "Alias (2001 - 2003)",
                ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/4/20/56966d674b06d.jpg",
                Progress = 100,
                Publisher = "Marvel",
                PublisherIcon = "M",
                Color = "red",
                Shelf = Shelf.Finished
            },
            new LibrarySeries
            {
                Id = 5,
                Title = "Action Comics (2016-)",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439423/439423._SX312_QL80_TTD_.jpg",
                Progress = 20,
                Publisher = "DC Comics",
                PublisherIcon = "DC",
                Color = "blue",
                Shelf = Shelf.Archived
            },
            new LibrarySeries
            {
                Id = 6,
                Title = "Bitch Planet",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/294324/294324._SX360_QL80_TTD_.jpg",
                Progress = 15,
                Publisher = "Image Comics",
                PublisherIcon = "I",
                Color = "pink",
                Shelf = Shelf.Archived
            }
        };

        public async Task<List<Model.Series>> GetShelf(Shelf shelf, bool refreshCache)
        {
            return AllSeries
                .Where(s => s.Shelf == shelf)
                .Select(s => new Model.Series(s))
                .ToList();
        }

        public async Task<bool> UpdateShelf(Model.Series series, Shelf newShelf)
        {
            var seriesToMove = AllSeries.Single(s => s.Id == series.Id);
            seriesToMove.Shelf = newShelf;
            if (newShelf == Shelf.Finished)
            {
                seriesToMove.Progress = 100;
            }
            else if (newShelf == Shelf.Unread)
            {
                seriesToMove.Progress = 0;
            }

            return true;
        }
    }
}
