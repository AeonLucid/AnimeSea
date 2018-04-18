using Microsoft.AspNetCore.Mvc;

namespace AnimeSea.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
