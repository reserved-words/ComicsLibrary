using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockReadingRepository : IReadingRepository
    {
        public async Task<List<NextComicInSeries>> GetNextToRead(bool refreshCache)
        {
            return new List<NextComicInSeries>
            {
                new NextComicInSeries
                {
                    Id = 105,
                    SeriesId = 1,
                    SeriesTitle = "Black Widow",
                    Years = "2020 - Present",
                    Publisher = "M",
                    Color = "red",
                    IssueTitle = "#5",
                    ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/9/70/6026d186cfbc7.jpg",
                    ReadUrl = "https://read.marvel.com/#/book/56005",
                    UnreadBooks = 2,
                    Creators = "Kelly Thompson",
                    Progress = 45
                },
                new NextComicInSeries
                {
                    Id = 201,
                    SeriesId = 2,
                    SeriesTitle = "Captain Marvel",
                    Years = "2002 - 2004",
                    Publisher = "M",
                    Color = "red",
                    IssueTitle = "#1",
                    ImageUrl = "https://i.annihil.us/u/prod/marvel/i/mg/f/a0/5b17d999abbdb.jpg",
                    ReadUrl = "https://read.marvel.com/#/book/10389",
                    UnreadBooks = 25,
                    Creators = "Kelly Thompson",
                    Progress = 0
                },
                new NextComicInSeries
                {
                    Id = 301,
                    SeriesId = 3,
                    SeriesTitle = "Harley Quinn",
                    Years = "2016 - Present",
                    Publisher = "DC",
                    Color = "blue",
                    IssueTitle = "Vol. 1",
                    ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/439438/439438._SX312_QL80_TTD_.jpg",
                    ReadUrl = "https://www.comixology.co.uk/comic-reader/74153/439438",
                    UnreadBooks = 10,
                    Creators = "Amanda Conner",
                    Progress = 0
                }
            };
        }
    }
}
