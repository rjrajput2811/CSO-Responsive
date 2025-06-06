using CSO.Core.Models;

namespace CSO.Core.Repositories.UserRepo;

public interface IUserRepository
{
    Task<UserViewModel?> GetUserByIdAsync(int userId);
    Task<OperationResult> InsertUserAsync(UserViewModel user);
    Task<OperationResult> UpdateUserAsync(UserViewModel user);
    Task<OperationResult> DeleteUserAsync(int userId);
}
