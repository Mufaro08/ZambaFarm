using Microsoft.AspNetCore.Mvc;

namespace ZambaFarm.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
