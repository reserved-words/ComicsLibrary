using ComicsLibrary.Common.Data;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public interface ISourceSearcher
    {
        Task<PagedResult<SearchResult>> SearchByTitle(string title, int sortOrder, int limit, int page);
        Task<PagedResult<Book>> GetBooks(int sourceItemID, int limit, int offset);
    }
}
