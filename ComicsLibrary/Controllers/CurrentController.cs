using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class CurrentController : BaseController
    {
        public IActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Current");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}