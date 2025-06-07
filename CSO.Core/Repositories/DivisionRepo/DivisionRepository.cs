using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.DivisionRepo;

public class DivisionRepository : SqlTableRepository, IDivisionRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public DivisionRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<DivisionViewModel>> GetDivisionList()
    {
        try
        {
            var list = await _dbContext.Divisions
                .Select(x => new DivisionViewModel
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

    public async Task<Division?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<Division>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(Division division, bool returnCreatedRecord = false)
    {
        try
        {
            var divisionToCreate = new Division();
            divisionToCreate.Name = division.Name;
            divisionToCreate.AddedBy = division.AddedBy;
            divisionToCreate.AddedOn = division.AddedOn;
            return await base.CreateAsync<Division>(divisionToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(Division division, bool returnUpdatedRecord = false)
    {
        try
        {
            var divisionToCreate = await base.GetByIdAsync<Division>(division.Id);
            //featureBenefitRecordToCreate.Serial_No = updateFeatureBenefitRecord.Serial_No;
            divisionToCreate.Name = division.Name;
            divisionToCreate.UpdatedBy = division.UpdatedBy;
            divisionToCreate.UpdatedOn = division.UpdatedOn;
            return await base.UpdateAsync<Division>(divisionToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<Division>(Id);
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

            IQueryable<int> query = _dbContext.Divisions
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.Divisions
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
