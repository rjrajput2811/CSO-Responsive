using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.ComplaintTypeRepo;

public class ComplaintTypeRepository : SqlTableRepository, IComplaintTypeRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public ComplaintTypeRepository(CSOResponsiveDbContext dbContext,
                     ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ComplaintTypeViewModel>> GetComTypeList()
    {
        try
        {
            var list = await _dbContext.ComplaintTypes
                .Select(x => new ComplaintTypeViewModel
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

    public async Task<ComplaintType?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<ComplaintType>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(ComplaintType comType, bool returnCreatedRecord = false)
    {
        try
        {
            var comTypeToCreate = new ComplaintType();
            comTypeToCreate.Name = comType.Name;
            comTypeToCreate.AddedBy = comType.AddedBy;
            comTypeToCreate.AddedOn = comType.AddedOn;
            return await base.CreateAsync<ComplaintType>(comTypeToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(ComplaintType comType, bool returnUpdatedRecord = false)
    {
        try
        {
            var comTypeToCreate = await base.GetByIdAsync<ComplaintType>(comType.Id);
            comTypeToCreate.Name = comType.Name;
            comTypeToCreate.UpdatedBy = comType.UpdatedBy;
            comTypeToCreate.UpdatedOn = comType.UpdatedOn;
            return await base.UpdateAsync<ComplaintType>(comTypeToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<ComplaintType>(Id);
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

            IQueryable<int> query = _dbContext.ComplaintTypes
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.ComplaintTypes
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
