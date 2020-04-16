using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface ISourceSearcher
    {
        Task<PagedResult<SearchResult>> SearchByTitle(string title, int sortOrder, int limit, int page);
        Task<PagedResult<Book>> GetBooks(int sourceItemID, int limit, int offset);
    }
}
