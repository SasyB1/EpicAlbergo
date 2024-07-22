using Microsoft.AspNetCore.Mvc;

namespace EpicAlbergo.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
