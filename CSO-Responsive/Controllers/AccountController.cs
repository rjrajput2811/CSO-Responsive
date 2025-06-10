using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.UserRepo;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _usersRepository;
        public AccountController(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var loginUser = await _usersRepository.Login(user);
                if (loginUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", loginUser.Id);
                    HttpContext.Session.SetString("FullName", loginUser.Name ?? "");
                    HttpContext.Session.SetInt32("UserRole", (int)loginUser.RoleId);

                        return RedirectToAction("Index", "Home");
                    
                    
                }
                ModelState.AddModelError("WrongCredentials", "Incorrect email address or password.");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
