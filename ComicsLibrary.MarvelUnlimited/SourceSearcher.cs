using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MarvelSharp;
using Microsoft.Extensions.Configuration;
using MarvelSharp.Model;
using MarvelSharp.Criteria;
using System.Linq;

namespace ComicsLibrary.MarvelUnlimited
{
    public class SourceSearcher : ISourceSearcher
    {
        private readonly ApiService _api;
        private readonly IMapper _mapper;

        public SourceSearcher(IConfiguration config, IMapper mapper)
        {
            _api = new ApiService(config["MarvelApiPublicKey"], config["MarvelApiPrivateKey"]);
            _mapper = mapper;
        }

        public async Task<PagedResult<Book>> GetBooks(int sourceItemID, int limit, int offset)
        {
            var page = offset / limit + 1;
            var result = await _api.GetSeriesComicsAsync(sourceItemID, limit, offset, new ComicCriteria { OrderBy = new List<ComicOrder> 
                { 
                    ComicOrder.IssueNumberAscending 
                }, 
                NoVariants = true
            });
            var comics = result.Data.Result.Select(r => _mapper.Map(r)).ToList();
            return new PagedResult<Book>(comics, limit, page, result.Data.Total ?? comics.Count);
        }

        public async Task<PagedResult<SearchResult>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            var results = await GetSeries(title, limit, page, (SearchOrder)sortOrder);

            var list = new List<SearchResult>();

            if (!results.Success)
                throw new Exception("Search failed");

            foreach (var result in results.Data.Result)
            {
                if (!result.Id.HasValue)
                    continue;

                list.Add(new SearchResult
                {
                    SourceId = 1,
                    SourceItemId = result.Id.Value,
                    Title = result.Title,
                    Url = result.Urls.FirstOrDefault()?.Value.Secure(),
                    ImageUrl = result.Thumbnail.MapToString().Secure()
                });
            }
            return new PagedResult<SearchResult>(list, limit, page, results.Data.Total ?? list.Count());
        }

        private async Task<Response<List<MarvelSharp.Model.Series>>> GetSeries(string titleStartsWith, int limit, int page, SearchOrder? orderBy)
        {
            return await _api.GetAllSeriesAsync(limit, (page - 1) * limit, new SeriesCriteria
            {
                TitleStartsWith = titleStartsWith,
                OrderBy = new List<SeriesOrder>
                {
                    orderBy == SearchOrder.StartYearDescending
                        ? SeriesOrder.StartYearDescending
                        : SeriesOrder.TitleAscending
                },
                Contains = new List<Format>
                {
                    Format.Comic,
                    Format.DigitalComic,
                    Format.InfiniteComic
                }
            });
        }
    }
}
