using CSO.Core.Models;
using CSO.Core.Repositories.SecurityActionRepo;
using CSO.Core.Repositories.UsersRoleRepo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSO_Responsive.Controllers;

public class SecurityActionController : Controller
{
    private readonly ISecurityActionRepository _securityActionRepository;
    private readonly IUsersRoleRepository _usersRoleRepository;

    public SecurityActionController(ISecurityActionRepository securityActionRepository,
                                    IUsersRoleRepository usersRoleRepository)
    {
        _securityActionRepository = securityActionRepository;
        _usersRoleRepository = usersRoleRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SecurityActionDetailsAsync(int id)
    {
        var model = new SecurityActionsPageViewModel();
        model.SecurityActionsViewModel = await _securityActionRepository.GetSecurityActionListBuRoleIdAsync(id);
        return View(model);
    }

    public async Task<ActionResult> GetRoleListAsync()
    {
        var list = await _securityActionRepository.GetRoleListForSecurityActionAsync();
        return Json(list);
    }

    public async Task<ActionResult> InsertUpdateSecurityActoionAsync(List<SecurityActionViewModel> model, int roleId, string roleName)
    {
        var response = await _usersRoleRepository.CheckUserRoleNameExist(roleName);
        if (!response)
        {
            return Json(new OperationResult
            {
                Success = false,
                Message = "Role already exist. Please enter unique role."
            });
        }

        foreach (var item in model)
        {
            item.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            item.AddedOn = DateTime.Now;
        }

        if (roleId > 0)
        {
            var result = await _securityActionRepository.DeleteSecurityActionAsync(roleId);
            if(!result.Success) { return Json(result); }
        }

        var insertUpdateResult = await _securityActionRepository.CreateSecurityActionAsync(model);

        return Json(insertUpdateResult);
    }

    public async Task<ActionResult> DeleteSecurityActoionAsync(int roleId)
    {
        var result = await _securityActionRepository.DeleteSecurityActionAsync(roleId);
        return Json(result);
    }
}
