using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class LibraryController : BaseController
    {
        public IActionResult Index(string id = null)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Library");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}