using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
