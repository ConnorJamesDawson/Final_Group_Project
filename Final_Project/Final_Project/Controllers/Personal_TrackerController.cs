using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class Personal_TrackerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
