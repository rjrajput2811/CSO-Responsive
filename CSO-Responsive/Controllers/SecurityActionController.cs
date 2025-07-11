using CSO.Core.DatabaseContext;
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

    public async Task<IActionResult> SecurityActionDetails(int id)
    {
        var model = new SecurityActionsPageViewModel();
        if(id > 0)
        {
            var userRole = await _usersRoleRepository.GetUserRoleByIdAsync(id);
            model.UserRoleId = userRole.Id;
            model.UserRoleName = userRole.RoleName;
        }
        model.SecurityActionsViewModel = await _securityActionRepository.GetSecurityActionListByRoleIdAsync(id);
        return View(model);
    }

    public async Task<ActionResult> GetRoleListAsync()
    {
        var list = await _securityActionRepository.GetRoleListForSecurityActionAsync();
        return Json(list);
    }

    public async Task<ActionResult> InsertUpdateSecurityActoionAsync([FromBody] List<SecurityActionViewModel> model)
    {
        var roleName = model.Select(x => x.UserRoleName).FirstOrDefault();
        var roleId = model.Select(x => x.UserRoleId).FirstOrDefault();

        foreach (var item in model)
        {
            item.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            item.AddedOn = DateTime.Now;
        }

        if (roleId > 0)
        {
            var userRole = await _usersRoleRepository.GetUserRoleByIdAsync(roleId);

            if(userRole.RoleName != roleName)
            {
                var userRoleToUpdate = new UsersRoleViewModel
                {
                    RoleName = roleName,
                    UpdatedBy = HttpContext.Session.GetInt32("UserId") ?? 0,
                    UpdatedOn = DateTime.Now
                };

                var response = await _usersRoleRepository.UpdateUserRoleAsync(userRoleToUpdate);
                if (!response.Success) { return Json(response); }
            }

            var result = await _securityActionRepository.DeleteSecurityActionAsync(roleId);
            if(!result.Success) { return Json(result); }
        }
        else
        {

            var response = await _usersRoleRepository.CheckUserRoleNameExist(roleName);
            if (response)
            {
                return Json(new OperationResult
                {
                    Success = false,
                    Message = "Role already exist. Please enter unique role."
                });
            }

            var userRoleToCreate = new UsersRoleViewModel
            {
                RoleName = roleName,
                AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0,
                AddedOn = DateTime.Now
            };

            var result = await _usersRoleRepository.CreateUserRoleAsync(userRoleToCreate, true);
            if (!result.Success) { return Json(result); }

            var userRole = result.Payload as UsersRole;

            foreach (var item in model)
            {
                item.UserRoleId = userRole.Id; 
            }
        }

        var insertUpdateResult = await _securityActionRepository.CreateSecurityActionAsync(model);

        return Json(insertUpdateResult);
    }

    public async Task<ActionResult> DeleteSecurityActoionAsync(int roleId)
    {
        var result = await _securityActionRepository.DeleteSecurityActionAsync(roleId, true);
        return Json(result);
    }
}
