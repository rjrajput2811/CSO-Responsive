using CSO.Core.Models;
using CSO.Core.Repositories.SecurityActionRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Repositories.UsersRoleRepo;
using CSO.Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSO_Responsive.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserRepository _usersRepository;
        private readonly IUsersRoleRepository _usersRoleRepository;
        private readonly ISecurityActionRepository _securityActionRepository;
        public AccountController(IUserRepository usersRepository,
                                 IUserRepository userService,
                                 IUsersRoleRepository usersRoleRepository,
                              ISecurityActionRepository securityActionRepository)
        {
            _usersRepository = usersRepository;
            _usersRoleRepository = usersRoleRepository;
            _securityActionRepository = securityActionRepository;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var loginUser = await _usersRepository.Login(user);
                if (loginUser != null)
                {
                    var ua = HttpContext.Request.Headers["User-Agent"].ToString();
                    var isMobile = ua.Contains("Mobi") || ua.Contains("Android") || ua.Contains("iPhone");

                    // === Dashboard ===
                    var canDashboardShowOnMobile = isMobile && await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_MOBILE_DASHBOARD, loginUser.RoleId);
                    var canDashboardShowOnDesktop = !isMobile && await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_DESKTOP_DASHBOARD, loginUser.RoleId);
                    var canViewDashboard = await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_VIEW_DASHBOARD, loginUser.RoleId);

                    // === CSOLOG ===
                    var canCSOLogShowOnMobile = isMobile && await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_MOBILE_CSOLOG, loginUser.RoleId);
                    var canCSOLogShowOnDesktop = !isMobile && await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_DESKTOP_CSOLOG, loginUser.RoleId);
                    var canViewCSOLog = await _securityActionRepository.CanDoAsync(SecurityActionsEnum.SEC_VIEW_CSOLOG, loginUser.RoleId);

                    HttpContext.Session.SetInt32("UserId", loginUser.Id);
                    HttpContext.Session.SetInt32("Role", loginUser.RoleId);
                    HttpContext.Session.SetString("FullName", loginUser.Name ?? "");
                    HttpContext.Session.SetInt32("UserRole", loginUser.RoleId);
                    HttpContext.Session.SetString("RoleName", await _usersRoleRepository.GetRoleName(loginUser.RoleId));
                    HttpContext.Session.SetString("Designation", loginUser.Designation);
                    HttpContext.Session.SetInt32("UserType", loginUser.UserType);

                    if (DateTime.Now.Month > 3)
                    {
                        HttpContext.Session.SetString("FYear", (DateTime.Now.Year.ToString().Substring(2) + (DateTime.Now.Year + 1).ToString().Substring(2)));
                    }
                    else
                    {
                        HttpContext.Session.SetString("FYear", ((DateTime.Now.Year - 1).ToString().Substring(2) + (DateTime.Now.Year).ToString().Substring(2)));
                    }

                    // ✅ Create user claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginUser.Name),
                        new Claim(ClaimTypes.Email, loginUser.Email),
                        new Claim(ClaimTypes.Role, await _usersRoleRepository.GetRoleName(loginUser.RoleId)),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = user.RememberMe
                    };

                    // ✅ Sign in (this creates the auth cookie)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        authProperties
                    );

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl != "/")
                        return Redirect(returnUrl);

                    if ((canDashboardShowOnMobile || canDashboardShowOnDesktop) && canViewDashboard)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if ((canCSOLogShowOnMobile || canCSOLogShowOnDesktop) && canViewCSOLog)
                    {
                        return RedirectToAction("Index", "CSOLog");
                    }
                    else
                    {
                        return RedirectToAction("Welcome", "Home");
                    }

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
