using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CSO.Core.Repositories.UserRepo;

public class UserRepository : SqlTableRepository, IUserRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public UserRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<User> Login(LoginViewModel loginViewModel)
    {
        try
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == loginViewModel.Username && x.Password == loginViewModel.Password);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<User> LoginWithAdId(string AdId)
    {
        try
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.ADid == AdId);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
    public async Task<UserViewModel?> GetUserByIdAsync(int userId)
    {
        try
        {
            var result = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Designation = x.Designation,
                    Email = x.Email,
                    MobileNo = x.MobileNo,
                    RoleId = x.RoleId,
                    UserType = x.UserType,
                    DivisionId = x.DivisionId,
                    PlantId = x.PlantId,
                    NearestPlantId = x.NearestPlantId,
                    BrandId = x.BrandId,
                    ProductTypeId = x.ProductTypeId,
                    IsInMailMatrix = x.IsInMailMatrix,
                    ADid = x.ADid
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> InsertUserAsync(UserViewModel user)
    {
        try
        {
            var userDetails = new User
            {
                Name = user.Name,
                Designation = user.Designation,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId,
                UserType = user.UserType,
                DivisionId = user.DivisionId,
                PlantId = user.PlantId,
                NearestPlantId = user.NearestPlantId,
                BrandId = user.BrandId,
                ProductTypeId = user.ProductTypeId,
                IsInMailMatrix = user.IsInMailMatrix,
                ADid = user.ADid,
                UserName = user.ADid,
                Password = user.ADid,
                AddedBy = user.AddedBy,
                AddedOn = user.AddedOn,
                Rights = ""
            };

            var result = await base.CreateAsync<User>(userDetails);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateUserAsync(UserViewModel user)
    {
        try
        {
            var userDetails = await base.GetByIdAsync<User>(user.Id);
            if (userDetails == null)
                return new OperationResult
                {
                    Success = false,
                    Message = "No record found for this user to update."
                };

            userDetails.Name = user.Name;
            userDetails.Designation = user.Designation;
            userDetails.Email = user.Email;
            userDetails.MobileNo = user.MobileNo;
            userDetails.RoleId = user.RoleId;
            userDetails.UserType = user.UserType;
            userDetails.DivisionId = user.DivisionId;
            userDetails.PlantId = user.PlantId;
            userDetails.NearestPlantId = user.NearestPlantId;
            userDetails.BrandId = user.BrandId;
            userDetails.ProductTypeId = user.ProductTypeId;
            userDetails.IsInMailMatrix = user.IsInMailMatrix;
            userDetails.ADid = user.ADid;
            userDetails.UserName = user.ADid;
            userDetails.Password = user.ADid;
            userDetails.UpdatedBy = user.UpdatedBy;
            userDetails.UpdatedOn = user.UpdatedOn;

            var result = await base.UpdateAsync<User>(userDetails);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteUserAsync(int userId)
    {
        try
        {
            var result = await base.DeleteAsync<User>(userId);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<UsersGridModel>> GetAllUsersAsync()
    {
        try
        {
            var list = await _dbContext.Users
                .Join(_dbContext.UserRoles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) => new UsersGridModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        ADid = user.ADid,
                        Email = user.Email,
                        MobileNo = user.MobileNo,
                        Designation = user.Designation,
                        Role = role.RoleName
                    }
                )
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
