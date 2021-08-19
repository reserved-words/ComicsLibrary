using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class Library : ILibrary
    {
        private readonly HttpClient _httpClient;

        public Library(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NextComicInSeries>> GetNextToRead()
        {
            var url = "http://localhost:58281/Library/GetAllNextUnread";
            var books = await _httpClient.GetFromJsonAsync<NextComicInSeries[]>(url);
            return books.ToList();
        }

        public async Task<List<Series>> GetShelf(int shelfId)
        {
            var url = $"http://localhost:58281/Library/Shelf?shelf={shelfId}";
            var result = await _httpClient.GetFromJsonAsync<LibrarySeries[]>(url);
            return result.Select(b => new Series(b)).ToList();
        }

        public async Task<NextComicInSeries> MarkReadAndGetNext(int bookId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<NextComicInSeries> MarkPreviousUnreadAndGet(int bookId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateShelf(int seriesId, int shelfId)
        {
            var url = $"http://localhost:58281/Library/Move";
            var body = new { id = seriesId, shelf = shelfId };
            var json = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, json);
            return response.IsSuccessStatusCode;
        }
    }
}
