using ComicsLibrary.Common;
using MarvelSharp;
using MarvelSharp.Criteria;
using MarvelSharp.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.MarvelUnlimited
{
    public class UpdateService : ISourceUpdateService
    {
        private const int MaxResults = 1000;
        private const int MaxResultsPerCall = 50;

        private readonly ApiService _api;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateService(IConfiguration config, ILogger logger, IMapper mapper)
        {
            _api = new ApiService(config["MarvelApiPublicKey"], config["MarvelApiPrivateKey"]);
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SeriesUpdate> GetUpdatedSeries(int sourceItemID, string url, bool isBackgroundProcess)
        {
            var seriesResponse = await _api.GetSeriesByIdAsync(sourceItemID);

            CheckSeriesResponse(seriesResponse, sourceItemID);

            var updatedSeries = _mapper.Map(seriesResponse.Data.Result);

            var comics = new List<MarvelSharp.Model.Comic>();
            var fetchMoreComics = true;

            while (fetchMoreComics)
            {
                fetchMoreComics = await AddComics(sourceItemID, comics);
            }

            updatedSeries.Books = comics
                .Select(c => _mapper.MapToUpdate(c))
                .Where(c => IsValid(c))
                .ToList();

            return updatedSeries;
        }

        private async Task<bool> AddComics(int id, List<MarvelSharp.Model.Comic> comics)
        {
            var offset = comics.Count;

            var response = await _api.GetSeriesComicsAsync(id, MaxResultsPerCall, offset, new ComicCriteria
            {
                OrderBy = new List<ComicOrder> { ComicOrder.IssueNumberAscending },
                NoVariants = true
            });

            CheckComicsResponse(response, id, offset);

            comics.AddRange(response.Data.Result);

            return response.Data.Total.HasValue
                && comics.Count < MaxResults
                && comics.Count < response.Data.Total;
        }

        private void CheckSeriesResponse(Response<Series> response, int id)
        {
            if (!response.Success)
            {
                var exception = new Exception($"Failed to fetch series {id}: {response.Status}");
                exception.Data.Add("SourceItemID", id);
                throw exception;
            }
        }

        private void CheckComicsResponse(Response<List<MarvelSharp.Model.Comic>> response, int id, int offset)
        {
            if (!response.Success)
            {
                var exception = new Exception($"Failed to fetch comics for series {id}: {response.Status}");
                exception.Data.Add("Offset", offset);
                exception.Data.Add("SourceItemID", id);
                throw exception;
            }

            if (response.Data.Total > MaxResults)
            {
                var exception = new Exception($"WARNING: Series has more than {MaxResults} comics");
                exception.Data.Add("TotalComics", response.Data.Total);
                exception.Data.Add("SourceItemID", id);
                _logger.Log(exception);
            }
        }

        private bool IsValid(BookUpdate book)
        {
            return !string.IsNullOrEmpty(book.ImageUrl) && !book.Title.EndsWith("Variant)");
        }
    }
}
