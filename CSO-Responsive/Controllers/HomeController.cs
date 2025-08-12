using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.SecurityActionRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Repositories.UsersRoleRepo;
using CSO.Core.Security;
using CSO.Core.Services.ActiveDirectoryUserRoleManagerService;
using CSO_Responsive.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CSO_Responsive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActiveDirectoryUserRoleManager _activeDirectoryUserRoleManager;
        private readonly IUserRepository _userRepository;
        private readonly IUsersRoleRepository _usersRoleRepository;
        private readonly ISecurityActionRepository _securityActionRepository;

        public HomeController(IActiveDirectoryUserRoleManager activeDirectoryUserRoleManager,
                              IUserRepository userRepository,
                              IUsersRoleRepository usersRoleRepository,
                              ISecurityActionRepository securityActionRepository)
        {
            _activeDirectoryUserRoleManager = activeDirectoryUserRoleManager;
            _userRepository = userRepository;
            _usersRoleRepository = usersRoleRepository;
            _securityActionRepository = securityActionRepository;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync(string? returnUrl = null)
        {
            string AdId = _activeDirectoryUserRoleManager.GetLoginUserName();
            if (!string.IsNullOrEmpty(AdId))
            {
                var loginUser = await _userRepository.LoginWithAdId(AdId);
                if(loginUser != null)
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

                    // ✅ Sign in (this creates the auth cookie)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal
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
                else
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
            }

            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetFinancialYear(string fyear)
        {
            HttpContext.Session.SetString("FYear", fyear);
            return Json(new { success = true });
        }
    }
}
