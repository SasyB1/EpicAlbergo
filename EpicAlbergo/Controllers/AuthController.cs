using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Interfaces;

namespace EpicAlbergo.Controllers
{

    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password")] UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = _userService.GetUser(model);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Username o password errati");
                return View();
            }
            _userService.Login(user);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            _userService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
