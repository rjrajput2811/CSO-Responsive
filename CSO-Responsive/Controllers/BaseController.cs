using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CSO_Responsive.Controllers;

[Authorize]
public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // ⛔ If not authenticated, redirect with returnUrl
        if (!User.Identity.IsAuthenticated)
        {
            var returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
            context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            return;
        }

        // ⛔ Optional: Check session presence
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            var returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
            context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            return;
        }

        base.OnActionExecuting(context);
    }
}
