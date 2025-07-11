using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CSO.Core.Repositories.SecurityActionRepo;

public class SecurityActionRepository : SqlTableRepository, ISecurityActionRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public SecurityActionRepository(CSOResponsiveDbContext dbContext,
                          ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<SecurityActionGridModel>> GetRoleListForSecurityActionAsync()
    {
        try
        {
            var list = await _dbContext.UserRoles
                .Select(x => new SecurityActionGridModel
                {
                    Id = x.Id,
                    RoleName = x.RoleName ?? "",
                    AddedBy = _dbContext.Users.FirstOrDefault(i => i.Id == x.AddedBy).Name ?? "",
                    AddedOn = x.AddedOn
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

    public async Task<List<SecurityActionViewModel>> GetSecurityActionListByRoleIdAsync(int roleId)
    {
        try
        {
            var list = await _dbContext.SecurityActions
                .Where(i => i.UserRoleId == roleId)
                .Select(x => new SecurityActionViewModel
                {
                    Id = x.Id,
                    UserRoleId = x.UserRoleId,
                    Action = x.Action
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

    public async Task<OperationResult> CreateSecurityActionAsync(List<SecurityActionViewModel> model)
    {
        try
        {
            var result = new OperationResult();
            var securityActionToCreateList = model.Select(x => new SecurityAction
            {
                UserRoleId = x.UserRoleId,
                Action = x.Action,
                AddedBy = x.AddedBy,
                AddedOn = x.AddedOn
            })
            .ToList();

            foreach(var securityActionToCreate in securityActionToCreateList)
            {
                result = await base.CreateAsync<SecurityAction>(securityActionToCreate);
                if (!result.Success) { return result; }
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteSecurityActionAsync(int RoleId, bool deleteUserRole = false)
    {
        try
        {
            var result = new OperationResult();
            var securityActionToDeleteList = await _dbContext.SecurityActions
                .Where(i => i.UserRoleId == RoleId)
                .ToListAsync();

            foreach(var securityActionToDelete in securityActionToDeleteList)
            {
                result = await base.DeleteAsync<SecurityAction>(securityActionToDelete.Id);
                if (!result.Success) { return result; }
            }

            if (deleteUserRole)
            {
                result = await base.DeleteAsync<UsersRole>(RoleId);
            }
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> CanDoAsync(SecurityActionsEnum securityAction, int roleId)
    {
        try
        {
            if (roleId == 7)
            {
                return true;
            }

            var result = await _dbContext.SecurityActions.AnyAsync(i => i.UserRoleId == roleId && i.Action == (int)securityAction);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
