using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Route("GetComics")]
        [HttpGet]
        public List<Comic> GetComics(int seriesId, int limit, int offset)
        {
            return _service.GetComics(seriesId, limit, offset);
        }

        [Route("AddToLibrary")]
        [HttpPost]
        public async Task<int> AddToLibrary([FromBody]Series series)
        {
            return await _service.AddSeriesToLibrary(series);
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
    }
}
