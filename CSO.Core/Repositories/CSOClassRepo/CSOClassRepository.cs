using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.CSOClassRepo;

public class CSOClassRepository : SqlTableRepository, ICSOClassRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CSOClassRepository(CSOResponsiveDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<CSOClassViewModel>> GetCsoClassList()
    {
        try
        {
            var list = await _dbContext.CSOClasses
                .Select(x => new CSOClassViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AddedOn = x.AddedOn,
                    AddedBy = x.AddedBy,
                    UpdatedOn = x.UpdatedOn,
                    UpdatedBy = x.UpdatedBy
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

    public async Task<CSOClass?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<CSOClass>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(CSOClass csoClass, bool returnCreatedRecord = false)
    {
        try
        {
            var csoClassToCreate = new CSOClass();
            csoClassToCreate.Name = csoClass.Name;
            csoClassToCreate.AddedBy = csoClass.AddedBy;
            csoClassToCreate.AddedOn = csoClass.AddedOn;
            return await base.CreateAsync<CSOClass>(csoClassToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(CSOClass csoClass, bool returnUpdatedRecord = false)
    {
        try
        {
            var csoClassToCreate = await base.GetByIdAsync<CSOClass>(csoClass.Id);
            csoClassToCreate.Name = csoClass.Name;
            csoClassToCreate.UpdatedBy = csoClass.UpdatedBy;
            csoClassToCreate.UpdatedOn = csoClass.UpdatedOn;
            return await base.UpdateAsync<CSOClass>(csoClassToCreate, returnUpdatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteAsync(int Id)
    {
        try
        {
            return await base.DeleteAsync<CSOClass>(Id);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> CheckDuplicate(string searchText, int Id)
    {
        try
        {
            bool existingflag = false;
            int? existingId = null;

            IQueryable<int> query = _dbContext.CSOClasses
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.CSOClasses
                    .Where(x =>
                           x.Name == searchText
                           && x.Id != Id)
                    .Select(x => x.Id);
            }


            existingId = await query.FirstOrDefaultAsync();

            if (existingId != null && existingId > 0)
            {
                existingflag = true;
            }

            return existingflag;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
