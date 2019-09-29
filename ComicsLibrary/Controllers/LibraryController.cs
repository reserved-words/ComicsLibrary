using System.Web.Mvc;

namespace ComicsLibrary.Controllers
{
    public class LibraryController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Library");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}