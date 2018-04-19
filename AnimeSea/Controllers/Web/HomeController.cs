using Microsoft.AspNetCore.Mvc;

namespace AnimeSea.Controllers.Web
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
