using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
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
                    CSOLogPhaseName = ((Status)x.CSOLogPhase).ToString(),
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

    public async Task<RecycleDayViewModel?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<RecycleDay>(Id);
            var finalResult = new RecycleDayViewModel
            {
                Id = result.Id,
                CSOLogPhase = result.CSOLogPhase,
                FromDate = result.FromDate,
                ToDate = result.ToDate,
                ThresholdDays = result.ThresholdDays
            };
            return finalResult;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(RecycleDayViewModel recycleDay, bool returnCreatedRecord = false)
    {
        try
        {
            var recycleDayToCreate = new RecycleDay
            {
                CSOLogPhase = recycleDay.CSOLogPhase,
                FromDate = recycleDay.FromDate,
                ToDate = recycleDay.ToDate,
                ThresholdDays = recycleDay.ThresholdDays,
                FinancialYear = recycleDay.FinancialYear,
                AddedBy = recycleDay.AddedBy,
                AddedDate = recycleDay.AddedDate
            };
            return await base.CreateAsync<RecycleDay>(recycleDayToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(RecycleDayViewModel recycleDay, bool returnUpdatedRecord = false)
    {
        try
        {
            var recycleDayToUpdate = await base.GetByIdAsync<RecycleDay>(recycleDay.Id);
            recycleDayToUpdate.CSOLogPhase = recycleDay.CSOLogPhase;
            recycleDayToUpdate.FromDate = recycleDay.FromDate;
            recycleDayToUpdate.ToDate = recycleDay.ToDate;
            recycleDayToUpdate.ThresholdDays = recycleDay.ThresholdDays;
            recycleDayToUpdate.ModifiedBy = recycleDay.ModifiedBy;
            recycleDayToUpdate.ModifiedDate = recycleDay.ModifiedDate;
            return await base.UpdateAsync<RecycleDay>(recycleDayToUpdate, returnUpdatedRecord);
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

    public async Task<bool> IsDateRangeOverlapping(int recDayId, int csoLogPhase, int financialYear, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var result = await _dbContext.RecycleDays
                .AnyAsync(i => i.FinancialYear == financialYear &&
                    (recDayId == 0 || i.Id != recDayId) && i.CSOLogPhase == csoLogPhase &&
                    (
                        (fromDate >= i.FromDate && fromDate <= i.ToDate) ||
                        (toDate >= i.FromDate && toDate <= i.ToDate) ||
                        (fromDate <= i.FromDate && toDate >= i.ToDate)
                    )
                );
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> IsDateUsedInLogCso(DateTime fromDate, DateTime toDate, int csoLogPhase)
    {
        try
        {
            var result = await _dbContext.CSOLogs
                .AnyAsync(i => i.Logdate >= fromDate && 
                               i.Logdate <= toDate && 
                               i.Status1 == csoLogPhase);
            return result;
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
