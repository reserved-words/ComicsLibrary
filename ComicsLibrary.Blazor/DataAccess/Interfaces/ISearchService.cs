using ComicsLibrary.Common;
using ComicsLibrary.Common.Data;
using System.Threading.Tasks;
using SearchResult = ComicsLibrary.Blazor.Model.SearchResult;

namespace ComicsLibrary.Blazor
{
    public interface ISearchService
    {
        Task<PagedResult<SearchResult>> Search(int sourceID, string text, int sortOrder, int limit, int page);
        Task<PagedResult<Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset);
    }
}
