using Microsoft.AspNetCore.Mvc;

namespace MoniteringBackend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
