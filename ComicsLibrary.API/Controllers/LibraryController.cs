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
    [Route("library")]
    public class LibraryController : ControllerBase
    {
        private readonly IService _service;

        public LibraryController(IService service)
        {
            _service = service;
        }

        [Route("GetNew")]
        [HttpGet]
        public List<Comic> GetNew(int limit)
        {
            return _service.GetLatestAdded(limit);
        }

        [Route("GetUpdated")]
        [HttpGet]
        public List<Comic> GetUpdated(int limit)
        {
            return _service.GetLatestUpdated(limit);
        }

        [Route("GetNext")]
        [HttpGet]
        public List<Series> GetNext()
        {
            return _service.GetToReadNext();
        }

        [Route("GetSeriesInProgress")]
        [HttpGet]
        public List<Series> GetSeriesInProgress()
        {
            return _service.GetSeriesInProgress();
        }

        [Route("GetSeriesToRead")]
        [HttpGet]
        public List<Series> GetSeriesToRead()
        {
            return _service.GetSeriesToRead();
        }

        [Route("GetSeriesFinished")]
        [HttpGet]
        public List<Series> GetSeriesFinished()
        {
            return _service.GetSeriesFinished();
        }

        [Route("GetSeriesAbandoned")]
        [HttpGet]
        public List<Series> GetSeriesAbandoned()
        {
            return _service.GetSeriesAbandoned();
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
        public async Task<int> AddToLibrary(Series series)
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
        public void AbandonSeries(int id)
        {
            _service.AbandonSeries(id);
        }

        [Route("ReinstateSeries")]
        [HttpPost]
        public void ReinstateSeries(int id)
        {
            _service.ReinstateSeries(id);
        }

        [Route("MarkAsRead")]
        [HttpPost]
        public IActionResult MarkAsRead(int[] ids)
        {
            if (ids != null)
            {
                _service.MarkAsRead(ids);
            }

            return Ok();
        }

        [Route("MarkAsUnread")]
        [HttpPost]
        public IActionResult MarkAsUnread(int[] ids)
        {
            if (ids != null)
            {
                _service.MarkAsUnread(ids);
            }

            return Ok();
        }

        [Route("AddToReadNext")]
        [HttpPost]
        public IActionResult AddToReadNext(int[] ids)
        {
            if (ids != null)
            {
                _service.AddToReadNext(ids);
            }

            return Ok();
        }

        [Route("RemoveFromReadNext")]
        [HttpPost]
        public IActionResult RemoveFromReadNext(int[] ids)
        {
            if (ids != null)
            {
                _service.RemoveFromReadNext(ids);
            }

            return Ok();
        }
    }
}
