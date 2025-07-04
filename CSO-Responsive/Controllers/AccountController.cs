using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Security;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;

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

                    if (DateTime.Now.Month > 3)
                    {
                        HttpContext.Session.SetString("FYear", (DateTime.Now.Year.ToString().Substring(2) + (DateTime.Now.Year + 1).ToString().Substring(2)));
                    }
                    else
                    {
                        HttpContext.Session.SetString("FYear", ((DateTime.Now.Year - 1).ToString().Substring(2) + (DateTime.Now.Year).ToString().Substring(2)));
                    }

                    return RedirectToAction("DashBoard", "DashBoard");
                    
                    
                }
                ModelState.AddModelError("WrongCredentials", "Incorrect email address or password.");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<JsonResult> SendEmailToForgotPassword(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return Json(new JsonModel(JsonType.Warning, "Username (email) is required."));
                }

                var result = await _usersRepository.SendEmailToForgotPassword(username);

                return result switch
                {
                    1 => Json(new JsonModel(JsonType.Success, "Check your email for password reset instructions.")),
                    -1 => Json(new JsonModel(JsonType.Warning, "Email service failed. Please contact the administrator.")),
                    0 => Json(new JsonModel(JsonType.Warning, "User not found with the provided email address.")),
                    _ => Json(new JsonModel(JsonType.Error, GlobalConstant.Error))
                };
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return Json(new JsonModel(JsonType.Error, $"An error occurred: {ex.Message}"));
            }
        }

        public IActionResult ForgotPassword(string id = null)
        {
            return View();
        }

        public async Task<JsonResult> SendOTPEmailToForgotPassword(string username)
        {
            try
            {
                Random random = new Random();
                string otp = random.Next(1000, 9999).ToString();
                var result = await _usersRepository.SendOTPEmailToForgotPassword(username, otp);

                if (result == 1)
                {
                    return Json(new JsonModel(JsonType.Success, "Check email for new password ", "", otp));
                }
                else if (result == 0)
                {
                    return Json(new JsonModel(JsonType.Warning, "You Have enter wrong email"));
                }
                else if (result == -1)
                {
                    return Json(new JsonModel(JsonType.Warning, "Email Service is not working . please contact administrator"));
                }
                else
                {
                    return Json(new JsonModel(JsonType.Error, GlobalConstant.Error));
                }


            }
            catch (Exception ex)
            {
                return Json(new JsonModel(JsonType.Error, ex.Message));
            }
        }

        public IActionResult ChangePassword(string username)
        {
            ViewBag.username = username;
            return View();
        }

        public async Task<JsonResult> UpdateNewPassword(string username, string password)
        {
            try
            {

                var result = await _usersRepository.ChangePassword(username, password);

                if (result == 1)
                {
                    return Json(new JsonModel(JsonType.Success, "Your password change successfully "));
                }
                else if (result == -1)
                {
                    return Json(new JsonModel(JsonType.Warning, "Update not done successfully"));
                }
                else
                {
                    return Json(new JsonModel(JsonType.Error, GlobalConstant.Error));
                }


            }
            catch (Exception ex)
            {
                return Json(new JsonModel(JsonType.Error, ex.Message));
            }
        }
    }
}
