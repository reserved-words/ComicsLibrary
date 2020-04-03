using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ComicsLibrary.Common.Models;
using ComicsLibrary.Common.Interfaces;

using Comic = ComicsLibrary.Common.Models.Comic;
using Series = ComicsLibrary.Common.Models.Series;
using Character = ComicsLibrary.Common.Models.Character;

using MarvelComic = MarvelSharp.Model.Comic;
using MarvelSeries = MarvelSharp.Model.Series;
using MarvelCharacter = MarvelSharp.Model.Character;

using MarvelSharp;
using MarvelSharp.Model;
using MarvelSharp.Criteria;

namespace ComicsLibrary.MarvelComicsApi
{
    public class Service : IApiService
    {
        private const int MaxResults = 1000;
        private const int MaxResultsPerCall = 30;

        private readonly IMapper _mapper;
        private readonly ApiService _apiService;

        public Service(IMapper mapper, IMarvelAppKeys appKeys)
        {
            _mapper = mapper;
            _apiService = new ApiService(appKeys.PublicKey, appKeys.PrivateKey);
        }

        public async Task<ApiResult<Series>> SearchSeriesAsync(string titleStartsWith, int limit, int page, SearchOrder? orderBy)
        {
            return new ApiResult<Series>
            {
                Success = true,
                Total = 100,
                Results = Enumerable.Range(1, limit)
                    .Select(i => new Series 
                    {
                        Id = 0,
                        MarvelId = i,
                        Title = "Series " + i,
                        StartYear = 1980,
                        EndYear = 1995,
                        Type = "test",
                        Url = "https://www.google.com/",
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/710793/710793._TTD_QL80_SX400_.jpg"
                    })
                    .ToList()
            };

            var series = await GetSeries(titleStartsWith, limit, page, orderBy);

            return _mapper.Map<Response<List<MarvelSeries>>, ApiResult<Series>>(series);
        }

        public async Task<ApiResult<Series>> GetCharacterSeriesAsync(int id, int limit, int page)
        {
            var series = await GetCharacterSeries(id, limit, page);

            return _mapper.Map<Response<List<MarvelSeries>>, ApiResult<Series>>(series);
        }

        public async Task<ApiResult<Comic>> GetSeriesComicsAsync(int id, int maxResults, int page)
        {
            var result = new ApiResult<Comic>
            {
                Success = true,
                Total = 65,
                Results = new List<Comic>()
            };

            for (var i = (page - 1) * 12; i < Math.Min(page * 12, 65); i++)
            {
                result.Results.Add(new Comic
                {
                    Title = "#" + i + 1,
                    IssueNumber = i + 1,
                    ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/cmx-images-prod/Item/710793/710793._TTD_QL80_SX400_.jpg",
                    ReadUrl = "https://www.google.com/",
                    OnSaleDate = DateTime.Now.AddDays(i-65 * 7)
                });
            }

            return result;

            var comics = await GetSeriesComics(id, maxResults, page);

            return _mapper.Map<Response<List<MarvelComic>>, ApiResult<Comic>>(comics);
        }



        public async Task<List<Comic>> GetAllSeriesComicsAsync(int id)
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

            return _mapper.Map<List<MarvelComic>, List<Comic>>(comics);
        }

        public async Task<string> GetAllSeriesCharactersAsync(int id)
        {
            var pageNo = 1;

            var response = await GetSeriesCharacters(id, MaxResultsPerCall, pageNo);
            CheckResponse(response);

            var characters = response.Data.Result;

            if (response.Data.Total.HasValue && response.Data.Total < MaxResults)
            {
                while (characters.Count < response.Data.Total)
                {
                    pageNo++;
                    var newResponse = await GetSeriesCharacters(id, MaxResultsPerCall, pageNo);
                    CheckResponse(newResponse);
                    characters.AddRange(newResponse.Data.Result);
                }
            }

            return string.Join(", ", characters.Select(c => c.Name));
        }

        public async Task<ApiResult<Character>> SearchCharacterAsync(string nameStartsWith, int limit, int page)
        {
            var characters = await GetCharacters(nameStartsWith, limit, page);

            return _mapper.Map<Response<List<MarvelCharacter>>, ApiResult<Character>>(characters);
        }

        private async Task<Response<List<MarvelCharacter>>> GetCharacters(string nameStartsWith, int limit, int page)
        {
            return await _apiService.GetAllCharactersAsync(limit, (page - 1) * limit, new CharacterCriteria
            {
                NameStartsWith = nameStartsWith,
                OrderBy = new List<CharacterOrder> { CharacterOrder.NameAscending }
            });
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

        private async Task<Response<List<MarvelSeries>>> GetCharacterSeries(int id, int limit, int page)
        {
            return await _apiService.GetCharacterSeriesAsync(id, limit, (page - 1) * limit, new SeriesCriteria
            {
                OrderBy = new List<SeriesOrder> { SeriesOrder.StartYearDescending }
            });
        }

        private async Task<Response<List<MarvelComic>>> GetSeriesComics(int id, int limit, int page)
        {
            return await _apiService.GetSeriesComicsAsync(id, limit, (page-1)*limit, new ComicCriteria
            {
                OrderBy = new List<ComicOrder> { ComicOrder.IssueNumberAscending },
                NoVariants = true
            });
        }

        private async Task<Response<List<MarvelCharacter>>> GetSeriesCharacters(int id, int limit, int page)
        {
            return await _apiService.GetSeriesCharactersAsync(id, limit, (page - 1) * limit);
        }

        private static void CheckResponse<T>(Response<T> response)
        {
            if (!response.Success)
                throw new Exception(response.Status);
        }
    }
}