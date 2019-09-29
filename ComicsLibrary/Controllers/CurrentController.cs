using System.Web.Mvc;

namespace ComicsLibrary.Controllers
{
    public class CurrentController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Current");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}