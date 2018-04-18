using Microsoft.AspNetCore.Mvc;

namespace AnimeSea.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
