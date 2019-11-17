using System.Web.Mvc;

namespace ComicsLibrary.Controllers
{
    public class LatestController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Latest");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}