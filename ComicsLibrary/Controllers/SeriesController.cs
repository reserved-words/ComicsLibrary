using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class SeriesController : BaseController
    {
        public IActionResult Index(int id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Series");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}