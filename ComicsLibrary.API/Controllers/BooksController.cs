using ComicsLibrary.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.API.Controllers
{
    [Authorize]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly IService _service;

        public BooksController(IService service)
        {
            _service = service;
        }

        [Route("MarkAsRead")]
        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            _service.MarkAsRead(id);
            return Ok();
        }

        [Route("MarkAsUnread")]
        [HttpPost]
        public IActionResult MarkAsUnread(int id)
        {
            _service.MarkAsUnread(id);
            return Ok();
        }

        [Route("Hide")]
        [HttpPost]
        public IActionResult Hide(int id)
        {
            _service.HideBook(id, true);
            return Ok();
        }

        [Route("Unhide")]
        [HttpPost]
        public IActionResult Unhide(int id)
        {
            _service.HideBook(id, false);
            return Ok();
        }
    }
}
