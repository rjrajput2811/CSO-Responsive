using CSO.Core.Models;
using CSO.Core.Security;

namespace CSO.Core.Repositories.SecurityActionRepo;

public interface ISecurityActionRepository
{
    Task<List<SecurityActionGridModel>> GetRoleListForSecurityActionAsync();
    Task<List<SecurityActionViewModel>> GetSecurityActionListBuRoleIdAsync(int roleId);
    Task<OperationResult> CreateSecurityActionAsync(List<SecurityActionViewModel> model);
    Task<OperationResult> DeleteSecurityActionAsync(int RoleId);
    Task<bool> CanDoAsync(SecurityActionsEnum securityAction, int roleId);
}
