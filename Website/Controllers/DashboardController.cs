using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return File("~/dist/index.html", "text/html");
        }
    }
}
