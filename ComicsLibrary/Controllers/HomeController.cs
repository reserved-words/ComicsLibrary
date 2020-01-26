using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Callback()
        {
            return View();
        }
    }
}