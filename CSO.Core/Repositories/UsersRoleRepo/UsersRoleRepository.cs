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
}
