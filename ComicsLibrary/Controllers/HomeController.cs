using Microsoft.AspNetCore.Mvc;

namespace ComicsLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}