using ComicsLibrary.Common.Data;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public interface ISearchService
    {
        Task<PagedResult<SearchResult>> SearchByTitle(int sourceID, string title, int sortOrder, int limit, int page);
        Task<PagedResult<Book>> GetBooks(int sourceID, int sourceItemID, int limit, int offset);
        Task<int> AddToLibrary(SearchResult searchResult);
    }
}
