using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Models;
using EpicAlbergo.Services;

namespace EpicAlbergo.Controllers
{
   
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password")] User user)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var u = _userService.GetUser(user);
            if(u == null)
            {
                ModelState.AddModelError("Username", "Username o password errati");
                return View();
            }
            _userService.Login(u);
            return RedirectToAction("Index", "Home");
        }
    }
}
