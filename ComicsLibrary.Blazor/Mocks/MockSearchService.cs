using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common.Data;
using System;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Mocks
{
    public class MockSearchService : ISearchService
    {
        public async Task<Common.PagedResult<Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public async Task<Common.PagedResult<SearchResult>> Search(int sourceID, string text, int sortOrder, int limit, int page)
        {
            throw new NotImplementedException();
        }
    }
}
