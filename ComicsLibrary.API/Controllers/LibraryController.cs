using System.Collections.Generic;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HomeBookType = ComicsLibrary.Common.Data.HomeBookType;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("library")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService service)
        {
            _service = service;
        }

        [Route("GetAllNextUnread")]
        [HttpGet]
        public List<NextComicInSeries> GetAllNextUnread()
        {
            return _service.GetAllNextIssues();
        }

        [Route("GetNextUnread")]
        [HttpGet]
        public NextComicInSeries GetNextUnread(int seriesId)
        {
            return _service.GetNextUnread(seriesId);
        }

        [Route("GetProgress")]
        [HttpGet]
        public int GetProgress(int seriesId)
        {
            return _service.GetProgress(seriesId);
        }

        [Route("SetHomeOption")]
        [HttpPost]
        public void SetHomeOption([FromBody]HomeBookType homeBookType)
        {
            _service.UpdateHomeBookType(homeBookType);
        }

        [Route("Shelves")]
        [HttpGet]
        public List<LibraryShelf> GetShelves()
        {
            return _service.GetShelves();
        }

        [Route("Series")]
        [HttpGet]
        public LibrarySeries GetSeries(int id)
        {
            return _service.GetSeries(id);
        }
    }
}
