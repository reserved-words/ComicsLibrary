using ComicsLibrary.Common.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class APIController : ControllerBase
    {
        private readonly IService _service;

        public APIController(IService service)
        {
            _service = service;
        }

        [Route("API/GetNew")]
        [HttpGet]
        public List<Comic> GetNew(int limit)
        {
            return _service.GetLatestAdded(limit);
        }

        [Route("API/GetUpdated")]
        [HttpGet]
        public List<Comic> GetUpdated(int limit)
        {
            return _service.GetLatestUpdated(limit);
        }

        [Route("API/GetNext")]
        [HttpGet]
        public List<Series> GetNext()
        {
            return _service.GetToReadNext();
        }

        [Route("API/GetSeriesInProgress")]
        [HttpGet]
        public List<Series> GetSeriesInProgress()
        {
            return _service.GetSeriesInProgress();
        }

        [Route("API/GetSeriesToRead")]
        [HttpGet]
        public List<Series> GetSeriesToRead()
        {
            return _service.GetSeriesToRead();
        }

        [Route("API/GetSeriesFinished")]
        [HttpGet]
        public List<Series> GetSeriesFinished()
        {
            return _service.GetSeriesFinished();
        }

        [Route("API/GetSeriesAbandoned")]
        [HttpGet]
        public List<Series> GetSeriesAbandoned()
        {
            return _service.GetSeriesAbandoned();
        }

        [Route("API/GetSeries")]
        [HttpGet]
        public Series GetSeries(int seriesId, int limit)
        {
            return _service.GetSeries(seriesId, limit);
        }

        [Route("API/GetComics")]
        [HttpGet]
        public List<Comic> GetComics(int seriesId, int limit, int offset)
        {
            return _service.GetComics(seriesId, limit, offset);
        }

        [Route("API/GetComicsByMarvelId")]
        [HttpGet]
        public async Task<PagedResult<Comic>> GetComicsByMarvelId(int marvelId, int limit, int offset)
        {
            return await _service.GetComicsByMarvelId(marvelId, limit, offset);
        }

        [Route("API/AddToLibrary")]
        [HttpPost]
        public async Task<int> AddToLibrary(Series series)
        {
            return await _service.AddSeriesToLibrary(series);
        }

        [Route("API/RemoveFromLibrary")]
        [HttpPost]
        public void RemoveFromLibrary(int seriesId)
        {
            _service.RemoveSeriesFromLibrary(seriesId);
        }

        [Route("API/AbandonSeries")]
        [HttpPost]
        public void AbandonSeries(int seriesId)
        {
            _service.AbandonSeries(seriesId);
        }

        [Route("API/ReinstateSeries")]
        [HttpPost]
        public void Reinstate(int seriesId)
        {
            _service.ReinstateSeries(seriesId);
        }

        [Route("API/MarkAsRead")]
        [HttpPost]
        public IActionResult MarkAsRead(int[] ids)
        {
            if (ids != null)
            {
                _service.MarkAsRead(ids);
            }

            return Ok();
        }

        [Route("API/MarkAsUnread")]
        [HttpPost]
        public IActionResult MarkAsUnread(int[] ids)
        {
            if (ids != null)
            {
                _service.MarkAsUnread(ids);
            }

            return Ok();
        }

        [Route("API/AddToReadNext")]
        [HttpPost]
        public IActionResult AddToReadNext(int[] ids)
        {
            if (ids != null)
            {
                _service.AddToReadNext(ids);
            }

            return Ok();
        }

        [Route("API/RemoveFromReadNext")]
        [HttpPost]
        public IActionResult RemoveFromReadNext(int[] ids)
        {
            if (ids != null)
            {
                _service.RemoveFromReadNext(ids);
            }

            return Ok();
        }

        [Route("API/SearchByTitle")]
        [HttpGet]
        public async Task<PagedResult<Series>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            return await _service.SearchByTitle(title, sortOrder, limit, page);
        }
    }
}
