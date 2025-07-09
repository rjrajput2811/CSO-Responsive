using CSO.Core.Models;

namespace CSO.Core.Repositories.UsersRoleRepo;

public interface IUsersRoleRepository
{
    Task<List<UsersRoleViewModel>> GetUserRolesAsync();

    Task<bool> CheckUserRoleNameExist(string rolename);
}
