using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.UsersRoleRepo;

public interface IUsersRoleRepository
{
    Task<List<UsersRoleViewModel>> GetUserRolesAsync();
    Task<bool> CheckUserRoleNameExist(string rolename);
    Task<OperationResult> CreateUserRoleAsync(UsersRoleViewModel model, bool retuenCreatedRecord = false);
    Task<OperationResult> UpdateUserRoleAsync(UsersRoleViewModel model);
    Task<OperationResult> DeleteUserRoleAsync(int Id);
    Task<UsersRole> GetUserRoleByIdAsync(int Id);
}
