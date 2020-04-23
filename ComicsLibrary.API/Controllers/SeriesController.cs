using System.Collections.Generic;
using ComicsLibrary.Common;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HomeBookType = ComicsLibrary.Common.Models.HomeBookType;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("series")]
    public class SeriesController : ControllerBase
    {
        private readonly IService _service;

        public SeriesController(IService service)
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

        [Route("GetByStatus")]
        [HttpGet]
        public List<Series> GetByStatus(SeriesStatus status)
        {
            return _service.GetSeriesByStatus(status);
        }

        [Route("GetByID")]
        [HttpGet]
        public Series GetByID(int seriesId, int limit)
        {
            return _service.GetSeries(seriesId, limit);
        }

        [Route("GetBooks")]
        [HttpGet]
        public List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            return _service.GetBooks(seriesId, typeId, limit, offset);
        }

        [Route("SetHomeOption")]
        [HttpPost]
        public void SetHomeOption([FromBody]HomeBookType homeBookType)
        {
            _service.UpdateHomeBookType(homeBookType);
        }

        [Route("Remove")]
        [HttpPost]
        public void Remove(int id)
        {
            _service.RemoveSeriesFromLibrary(id);
        }

        [Route("Archive")]
        [HttpPost]
        public IActionResult Archive(int id)
        {
            _service.ArchiveSeries(id);
            return Ok();
        }

        [Route("Reinstate")]
        [HttpPost]
        public IActionResult Reinstate(int id)
        {
            _service.ReinstateSeries(id);
            return Ok();
        }
    }
}
