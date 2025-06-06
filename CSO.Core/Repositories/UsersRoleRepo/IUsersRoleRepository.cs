using CSO.Core.Models;

namespace CSO.Core.Repositories.UsersRoleRepo;

public interface IUsersRoleRepository
{
    Task<List<UsersRoleViewModel>> GetUserRolesAsync();
}
