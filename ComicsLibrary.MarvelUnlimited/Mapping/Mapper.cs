using System.Linq;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Models;
using MarvelSharp.Model;

namespace ComicsLibrary.MarvelUnlimited
{
    public class Mapper : IMapper
    {
        public SeriesUpdate Map(MarvelSharp.Model.Series source)
        {
            return new SeriesUpdate
            {
                Title = source.Title,
                StartYear = source.StartYear,
                EndYear = source.EndYear,
                Url = source.Urls.FirstOrDefault()?.Value,
                ImageUrl = source.Thumbnail.MapToString()
            };
        }

        public BookUpdate MapToUpdate(Comic source)
        {
            return new BookUpdate
            {
                ImageUrl = source.Thumbnail.MapToString(),
                ReadUrl = source.GetReaderUrl(),
                SourceItemID = source.Id,
                Creators = source.GetCreators(),
                OnSaleDate = source.GetOnSaleDate(),
                Title = source.Title,
                Number = source.IssueNumber,
                BookTypeName = "Issues"
            };
        }

        public Book Map(Comic source)
        {
            return new Book
            {
                ImageUrl = source.Thumbnail.MapToString(),
                ReadUrl = source.GetReaderUrl(),
                SourceItemID = source.Id,
                Creators = source.GetCreators(),
                OnSaleDate = source.GetOnSaleDate(),
                Title = source.Title,
                Number = source.IssueNumber
            };
        }
    }
}
