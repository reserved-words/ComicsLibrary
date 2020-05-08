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

        public async Task<SeriesUpdate> GetUpdatedSeries(int sourceItemID, string url, bool isBackgroundProcess)
        {
            var series = await _service.GetUpdatedSeries(sourceItemID, url, isBackgroundProcess);

            return new SeriesUpdate
            {
                Title = series.Title,
                StartYear = series.StartYear,
                EndYear = series.EndYear,
                Url = series.Url.Secure(),
                Books = series.Books.Select(b => new BookUpdate
                {
                    BookTypeName = b.BookTypeName,
                    SourceItemID = b.SourceItemID,
                    Title = b.Title,
                    Number = b.Number,
                    ImageUrl = b.ImageUrl.Secure(),
                    ReadUrl = b.ReadUrl.Secure()
                })
                .ToList()
            };
        }
    }
}
