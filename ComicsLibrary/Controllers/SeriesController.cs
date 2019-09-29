using System.Web.Mvc;

namespace ComicsLibrary.Controllers
{
    public class SeriesController : Controller
    {
        public ActionResult Index(int id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Series");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}