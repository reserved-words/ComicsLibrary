using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Models;
using ComicsLibrary.Common;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISearchService _service;


        public SearchController(IConfiguration config, ISearchService service)
        {
            _config = config;
            _service = service;
        }

        [Route("GetSearchOptions")]
        [HttpGet]
        public object GetSearchOptions()
        {
            return _config.GetSection("Sources")
                .GetChildren()
                .Select(c => c.Get<SourceOption>())
                .ToList();
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
