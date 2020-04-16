using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;
using ComicsLibrary.Common;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _service;


        public SearchController(ISearchService service)
        {
            _service = service;
        }

        [Route("GetComics")]
        [HttpGet]
        public async Task<PagedResult<Book>> GetComics(int sourceID, int sourceItemID, int limit, int offset)
        {
            return await _service.GetBooks(sourceID, sourceItemID, limit, offset);
        }

        [Route("SearchByTitle")]
        [HttpGet]
        public async Task<PagedResult<SearchResult>> SearchByTitle(int sourceID, string title, int sortOrder, int limit, int page)
        {
            return await _service.SearchByTitle(sourceID, title, sortOrder, limit, page);
        }

        [Route("AddToLibrary")]
        [HttpPost]
        public async Task<int> AddToLibrary([FromBody]SearchResult series)
        {
            return await _service.AddToLibrary(series);
        }
    }
}
