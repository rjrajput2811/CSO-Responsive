using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CSO.Core.Services.ActiveDirectoryUserRoleManagerService;

public class ActiveDirectoryUserRoleManager : IActiveDirectoryUserRoleManager
{
    private HttpContext HttpContext { get; }

    public ActiveDirectoryUserRoleManager(IHttpContextAccessor httpContextAccessor)
    {
        HttpContext = httpContextAccessor.HttpContext;
    }


    public string GetLoginUserName()
    {
        string authenticatedUser = string.Empty;
        string userName = HttpContext.User.Identity.Name ?? "";
        if (userName.Contains("\\"))
            authenticatedUser = userName.Split('\\')[1];
        else
            authenticatedUser = userName;
        return authenticatedUser;
    }
}
