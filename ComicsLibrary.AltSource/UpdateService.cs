using ComicsLibrary.Common;
using ComicsLibrary.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.AltSource
{
    public class UpdateService : ISourceUpdateService
    {
        private readonly Comix.Services.UpdateService _service;

        public UpdateService(IConfiguration config)
        {
            _service = new Comix.Services.UpdateService(config);
        }

        public async Task<SeriesUpdate> GetUpdatedSeries(int sourceItemID, string url)
        {
            var series = await _service.GetUpdatedSeries(sourceItemID, url);

            return new SeriesUpdate
            {
                Title = series.Title,
                StartYear = series.StartYear,
                EndYear = series.EndYear,
                Url = series.Url,
                Books = series.Books.Select(b => new BookUpdate
                {
                    BookTypeName = b.BookTypeName,
                    SourceItemID = b.SourceItemID,
                    Title = b.Title,
                    Number = b.Number,
                    ImageUrl = b.ImageUrl,
                    ReadUrl = b.ReadUrl
                })
                .ToList()
            };
        }
    }
}
