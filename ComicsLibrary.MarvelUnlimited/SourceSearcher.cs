using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using ComicsLibrary.Common;
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

        public SourceSearcher(IConfiguration config)
        {
            _api = new ApiService(config["MarvelApiPublicKey"], config["MarvelApiPrivateKey"]);
        }

        public Task<PagedResult<Book>> GetBooks(int sourceItemID, int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<SearchResult>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            var results = await GetSeries(title, limit, page, (SearchOrder)sortOrder);

            var list = new List<SearchResult>();

            foreach (var result in results.Data.Result)
            {
                if (!result.Id.HasValue)
                    continue;

                list.Add(new SearchResult
                {
                    SourceId = 1,
                    SourceItemId = result.Id.Value,
                    Title = result.Title,
                    Url = result.Urls.FirstOrDefault()?.Value,
                    ImageUrl = MapImageToString(result.Thumbnail) // Move to mapper
                });
            }
            return new PagedResult<SearchResult>(list, limit, page, results.Data.Total ?? list.Count());
        }
        private const string NoImagePlaceholder = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available.jpg";

        private static string MapImageToString(Image image)
        {
            if (image == null || string.IsNullOrEmpty(image.Path) || string.IsNullOrEmpty(image.Extension))
                return string.Empty;

            var url = $"{image.Path}.{image.Extension}";

            if (url == NoImagePlaceholder)
                return string.Empty;

            return url;
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


        //public async Task<PagedResult<ApiComic>> GetComicsByMarvelId(int marvelId, int limit, int offset)
        //{
        //    try
        //    {
        //        var page = offset / limit + 1;
        //        var result = await _apiService.GetSeriesComicsAsync(marvelId, limit, page);
        //        var comics = _mapper.Map<List<Book>, List<ApiComic>>(result.Results);
        //        return new PagedResult<ApiComic>(comics, limit, page, result.Total);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(ex);
        //        return null;
        //    }
        //}

    }
}
