using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        public IActionResult PartialView(string viewName)
        {
            return new PartialViewResult
            {
                ViewName = viewName
            };
        }

        public IActionResult View()
        {
            return new ViewResult();
        }
    }
}
