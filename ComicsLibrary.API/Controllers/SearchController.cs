using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsLibrary.API.Controllers
{
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IService _service;

        public SearchController(IService service)
        {
            _service = service;
        }

        [Route("GetComicsByMarvelId")]
        [HttpGet]
        public async Task<PagedResult<Comic>> GetComicsByMarvelId(int marvelId, int limit, int offset)
        {
            return await _service.GetComicsByMarvelId(marvelId, limit, offset);
        }

        [Route("SearchByTitle")]
        [HttpGet]
        public async Task<PagedResult<Series>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            return await _service.SearchByTitle(title, sortOrder, limit, page);
        }
    }
}
