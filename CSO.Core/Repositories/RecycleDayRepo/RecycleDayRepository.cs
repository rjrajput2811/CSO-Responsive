using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.RecycleDayRepo;

public class RecycleDayRepository : SqlTableRepository, IRecycleDayRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public RecycleDayRepository(CSOResponsiveDbContext dbContext,
        ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<RecycleDayViewModel>> GetRecycleDayList()
    {
        try
        {
            var list = await _dbContext.RecycleDays
                .Select(x => new RecycleDayViewModel
                {
                    Id = x.Id,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    ThresholdDays = x.ThresholdDays,
                    FinancialYear = x.FinancialYear,
                    AddedDate = x.AddedDate,
                    AddedBy = x.AddedBy,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate
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

    public async Task<RecycleDay?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<RecycleDay>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(RecycleDay recycleDay, bool returnCreatedRecord = false)
    {
        try
        {
            var recycleDayToCreate = new RecycleDay();
            recycleDayToCreate.FromDate = recycleDay.FromDate;
            recycleDayToCreate.ToDate = recycleDay.ToDate;
            recycleDayToCreate.ThresholdDays = recycleDay.ThresholdDays;
            recycleDayToCreate.FinancialYear = recycleDay.FinancialYear;
            recycleDayToCreate.AddedBy = recycleDay.AddedBy;
            recycleDayToCreate.AddedDate = recycleDay.AddedDate;
            return await base.CreateAsync<RecycleDay>(recycleDayToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(RecycleDay recycleDay, bool returnUpdatedRecord = false)
    {
        try
        {
            var recycleDayToCreate = await base.GetByIdAsync<RecycleDay>(recycleDay.Id);
            recycleDayToCreate.FromDate = recycleDay.FromDate;
            recycleDayToCreate.ToDate = recycleDay.ToDate;
            recycleDayToCreate.ThresholdDays = recycleDay.ThresholdDays;
            recycleDayToCreate.ModifiedBy = recycleDay.ModifiedBy;
            recycleDayToCreate.ModifiedDate = recycleDay.ModifiedDate;
            return await base.UpdateAsync<RecycleDay>(recycleDayToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<RecycleDay>(Id);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    //public async Task<bool> CheckDuplicate(string searchText, int Id)
    //{
    //    try
    //    {
    //        bool existingflag = false;
    //        int? existingId = null;

    //        IQueryable<int> query = _dbContext.RecycleDays
    //            .Where(x => x.Name == searchText)
    //            .Select(x => x.Id);

    //        // Add additional condition if Id is not 0
    //        if (Id != 0)
    //        {
    //            query = _dbContext.Divisions
    //                .Where(x =>
    //                       x.Name == searchText
    //                       && x.Id != Id)
    //                .Select(x => x.Id);
    //        }


    //        existingId = await query.FirstOrDefaultAsync();

    //        if (existingId != null && existingId > 0)
    //        {
    //            existingflag = true;
    //        }

    //        return existingflag;
    //    }
    //    catch (Exception ex)
    //    {
    //        _systemLogService.WriteLog(ex.Message);
    //        throw;
    //    }
    //}
}
