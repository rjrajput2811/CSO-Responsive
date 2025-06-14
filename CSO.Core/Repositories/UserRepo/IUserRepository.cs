using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.UserRepo;

public interface IUserRepository
{
    Task<User> Login(LoginViewModel loginViewModel);
    Task<User> LoginWithAdId(string AdId);
    //Task<TabulatorResult> GetAllUsersAsync(TabulatorRequest request);
    Task<List<UsersGridModel>> GetAllUsersAsync();
    Task<UserViewModel?> GetUserByIdAsync(int userId);
    Task<OperationResult> InsertUserAsync(UserViewModel user);
    Task<OperationResult> UpdateUserAsync(UserViewModel user);
    Task<OperationResult> DeleteUserAsync(int userId);
}
