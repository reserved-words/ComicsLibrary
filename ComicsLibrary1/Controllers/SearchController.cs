using System.Web.Mvc;

namespace ComicsLibrary.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Search");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}