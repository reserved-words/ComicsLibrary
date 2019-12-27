using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class LatestController : BaseController
    {
        public IActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Latest");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}