using ComicsLibrary.Blazor.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;

        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Common.PagedResult<Common.Data.Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset)
        {
            var url = $"http://localhost:58281/Search/GetComics?sourceID={sourceID}&sourceItemID={sourceItemID}&limit={limit}&offset={offset}";
            return await _httpClient.GetFromJsonAsync<Common.PagedResult<Common.Data.Book>>(url);
        }

        public async Task<Common.PagedResult<SearchResult>> Search(int sourceID, string text, int sortOrder, int limit, int page)
        {
            var url = $"http://localhost:58281/Search/SearchByTitle?sourceID={sourceID}&title={text}&sortOrder={sortOrder}&limit={limit}&page={page}";
            var results = await _httpClient.GetFromJsonAsync<Common.PagedResult<Common.SearchResult>>(url);

            var converted = results.Results.Select(r => new SearchResult
            {
                Id = r.LibraryId,
                SourceId = r.SourceId,
                SourceItemId = r.SourceItemId,
                Title = r.Title,
                Publisher = r.Publisher
            })
            .ToList();

            return new Common.PagedResult<SearchResult>(converted, limit, page, results.TotalResults);
        }
    }
}
