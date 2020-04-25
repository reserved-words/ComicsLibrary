using System.Collections.Generic;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("series")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService _service;

        public SeriesController(ISeriesService service)
        {
            _service = service;
        }

        [Route("GetByID")]
        [HttpGet]
        public SeriesBookLists GetByID(int seriesId, int limit)
        {
            return _service.GetById(seriesId, limit);
        }

        [Route("GetBooks")]
        [HttpGet]
        public List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset)
        {
            return _service.GetBooks(seriesId, typeId, limit, offset);
        }

        [Route("Remove")]
        [HttpPost]
        public void Remove(int id)
        {
            _service.Remove(id);
        }

        [Route("Archive")]
        [HttpPost]
        public IActionResult Archive(int id)
        {
            _service.Archive(id);
            return Ok();
        }

        [Route("Reinstate")]
        [HttpPost]
        public IActionResult Reinstate(int id)
        {
            _service.Reinstate(id);
            return Ok();
        }
    }
}
