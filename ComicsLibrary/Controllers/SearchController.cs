using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class SearchController : BaseController
    {
        public IActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Search");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}