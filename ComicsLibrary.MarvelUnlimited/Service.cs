using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ComicsLibrary.Common.Models;
using ComicsLibrary.Common.Interfaces;

using Series = ComicsLibrary.Common.Models.Series;

using MarvelComic = MarvelSharp.Model.Comic;
using MarvelSeries = MarvelSharp.Model.Series;

using MarvelSharp;
using MarvelSharp.Model;
using MarvelSharp.Criteria;
using Microsoft.Extensions.Configuration;
using ComicsLibrary.Common.Api;

namespace ComicsLibrary.MarvelUnlimited
{
    public class Service : IApiService
    {
        private const int MaxResults = 1000;
        private const int MaxResultsPerCall = 30;

        private readonly IMapper _mapper;
        private readonly ApiService _apiService;

        public Service(IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _apiService = new ApiService(config["MarvelApiPrivateKey"], config["MarvelApiPublicKey"]);
        }

        public async Task<ApiResult<Series>> SearchSeriesAsync(string titleStartsWith, int limit, int page, SearchOrder? orderBy)
        {
            var series = await GetSeries(titleStartsWith, limit, page, orderBy);

            return _mapper.Map<Response<List<MarvelSeries>>, ApiResult<Series>>(series);
        }

        public async Task<ApiResult<Book>> GetSeriesComicsAsync(int id, int maxResults, int page)
        {
            var comics = await GetSeriesComics(id, maxResults, page);

            return _mapper.Map<Response<List<MarvelComic>>, ApiResult<Book>>(comics);
        }

        public async Task<List<Book>> GetAllSeriesComicsAsync(int id)
        {
            var pageNo = 1;

            var response = await GetSeriesComics(id, MaxResultsPerCall, pageNo);
            CheckResponse(response);

            var comics = response.Data.Result;

            if (response.Data.Total.HasValue && response.Data.Total < MaxResults)
            {
                while (comics.Count < response.Data.Total)
                {
                    pageNo++;
                    var newResponse = await GetSeriesComics(id, MaxResultsPerCall, pageNo);
                    CheckResponse(newResponse);
                    comics.AddRange(newResponse.Data.Result);
                }
            }

            return _mapper.Map<List<MarvelComic>, List<Book>>(comics);
        }

        private async Task<Response<List<MarvelSeries>>> GetSeries(string titleStartsWith, int limit, int page, SearchOrder? orderBy)
        {
            return await _apiService.GetAllSeriesAsync(limit, (page - 1) * limit, new SeriesCriteria
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

        private async Task<Response<List<MarvelComic>>> GetSeriesComics(int id, int limit, int page)
        {
            return await _apiService.GetSeriesComicsAsync(id, limit, (page - 1) * limit, new ComicCriteria
            {
                OrderBy = new List<ComicOrder> { ComicOrder.IssueNumberAscending },
                NoVariants = true
            });
        }

        private static void CheckResponse<T>(Response<T> response)
        {
            if (!response.Success)
                throw new Exception(response.Status);
        }
    }
}