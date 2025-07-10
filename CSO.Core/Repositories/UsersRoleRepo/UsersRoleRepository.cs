using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.UsersRoleRepo;

public class UsersRoleRepository : SqlTableRepository, IUsersRoleRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public UsersRoleRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public Task<List<UsersRoleViewModel>> GetUserRolesAsync()
    {
        try
        {
            var list = _dbContext.UserRoles
                .Select(x => new UsersRoleViewModel
                {
                    Id = x.Id,
                    RoleName = x.RoleName
                })
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> CheckUserRoleNameExist(string rolename)
    {
        try
        {
            var result = await _dbContext.UserRoles
                .AnyAsync(i => i.RoleName == rolename);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateUserRoleAsync(UsersRoleViewModel model, bool retuenCreatedRecord = false)
    {
        try
        {
            var userRole = new UsersRole
            {
                RoleName = model.RoleName,
                AddedBy = model.AddedBy,
                AddedOn = model.AddedOn
            };
            var result = await base.CreateAsync<UsersRole>(userRole, retuenCreatedRecord);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateUserRoleAsync(UsersRoleViewModel model)
    {
        try
        {
            var userRole = await base.GetByIdAsync<UsersRole>(model.Id);

            userRole.RoleName = model.RoleName;
            userRole.UpdatedBy = model.UpdatedBy;
            userRole.UpdatedOn = model.UpdatedOn;

            var result = await base.UpdateAsync<UsersRole>(userRole);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteUserRoleAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<UsersRole>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<UsersRole> GetUserRoleByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<UsersRole>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
