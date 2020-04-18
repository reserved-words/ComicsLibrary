using System.Collections.Generic;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HomeBookType = ComicsLibrary.Common.Models.HomeBookType;


namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("library")]
    public class LibraryController : ControllerBase
    {
        private readonly IService _service;

        public LibraryController(IService service)
        {
            _service = service;
        }

        [Route("GetNext")]
        [HttpGet]
        public List<NextComicInSeries> GetNext()
        {
            return _service.GetAllNextIssues();
        }

        [Route("GetProgress")]
        [HttpGet]
        public int GetProgress(int seriesId)
        {
            return _service.GetProgress(seriesId);
        }

        [Route("GetNextInSeries")]
        [HttpGet]
        public NextComicInSeries GetNextInSeries(int seriesId)
        {
            return _service.GetFirstUnread(seriesId);
        }

        [Route("GetSeriesByStatus")]
        [HttpGet]
        public List<Series> GetSeriesByStatus(SeriesStatus status)
        {
            return _service.GetSeriesByStatus(status);
        }

        [Route("GetSeries")]
        [HttpGet]
        public Series GetSeries(int seriesId, int limit)
        {
            return _service.GetSeries(seriesId, limit);
        }

        [Route("GetBooks")]
        [HttpGet]
        public List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            return _service.GetBooks(seriesId, typeId, limit, offset);
        }

        [Route("GetComics")]
        [HttpGet]
        public List<Comic> GetComics(int seriesId, int limit, int offset)
        {
            return _service.GetComics(seriesId, limit, offset);
        }

        [Route("SetHomeOption")]
        [HttpPost]
        public void SetHomeOption([FromBody]HomeBookType homeBookType)
        {
            _service.UpdateHomeBookType(homeBookType);
        }

        [Route("RemoveFromLibrary")]
        [HttpPost]
        public void RemoveFromLibrary(int id)
        {
            _service.RemoveSeriesFromLibrary(id);
        }

        [Route("AbandonSeries")]
        [HttpPost]
        public IActionResult AbandonSeries(int id)
        {
            _service.AbandonSeries(id);
            return Ok();
        }

        [Route("ReinstateSeries")]
        [HttpPost]
        public IActionResult ReinstateSeries(int id)
        {
            _service.ReinstateSeries(id);
            return Ok();
        }

        [Route("MarkAsRead")]
        [HttpPost]
        public NextComicInSeries MarkAsRead(int id)
        {
            return _service.MarkAsRead(id);
        }

        [Route("MarkAsUnread")]
        [HttpPost]
        public IActionResult MarkAsUnread(int id)
        {
            _service.MarkAsUnread(id);
            return Ok();
        }

        [Route("HideBook")]
        [HttpPost]
        public IActionResult HideBook(int id)
        {
            _service.HideBook(id, true);
            return Ok();
        }

        [Route("UnhideBook")]
        [HttpPost]
        public IActionResult UnhideBook(int id)
        {
            _service.HideBook(id, false);
            return Ok();
        }
    }
}
