using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.CSOLogHistoryRepo;

public class CSOLogHistoryRepository : SqlTableRepository, ICSOLogHistoryRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public CSOLogHistoryRepository(CSOResponsiveDbContext dbContext,
                                   ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<bool> CheckCSOLogHistryExistsAsync(int csoLogId)
    {
        try
        {
            var existsLogHistory = await _dbContext.CSOLogHistories.AnyAsync(i => i.CSOLogId == csoLogId);
            return existsLogHistory;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateCSOLogHistoryAsync(CSOLogHistoryViewModel model)
    {
        try
        {
            var csoLogData = await base.GetByIdAsync<CSOLog>(model.CSOLogId);

            var csoLogHistory = new CSOLogHistory
            {
                CSOLogId = model.CSOLogId,
                CSOLogBy = csoLogData.AddedBy,
                CSOLogOn = csoLogData.AddedOn
            };

            var result = await base.CreateAsync<CSOLogHistory>(csoLogHistory);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateCSOLogHistoryForRootCauseAsync(CSOLogHistoryViewModel model)
    {
        try
        {
            var csoLogHistory = await _dbContext.CSOLogHistories.FirstOrDefaultAsync(i => i.CSOLogId == model.CSOLogId);

            csoLogHistory.RootCauseBy = model.RootCauseBy;
            csoLogHistory.RootCauseOn = model.RootCauseOn;

            var result = await base.UpdateAsync<CSOLogHistory>(csoLogHistory);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateCSOLogHistoryForMonitorAsync(CSOLogHistoryViewModel model)
    {
        try
        {
            var csoLogHistory = await _dbContext.CSOLogHistories.FirstOrDefaultAsync(i => i.CSOLogId == model.CSOLogId);

            csoLogHistory.MonitoringBy = model.MonitoringBy;
            csoLogHistory.MonitoringOn = model.MonitoringOn;

            var result = await base.UpdateAsync<CSOLogHistory>(csoLogHistory);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateCSOLogHistoryForApproveRejectAsync(CSOLogHistoryViewModel model)
    {
        try
        {
            var csoLogHistory = await _dbContext.CSOLogHistories.FirstOrDefaultAsync(i => i.CSOLogId == model.CSOLogId);

            csoLogHistory.ReviewBy = model.ReviewBy;
            csoLogHistory.ReviewOn = model.ReviewOn;

            var result = await base.UpdateAsync<CSOLogHistory>(csoLogHistory);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateCSOLogHistoryForCloseAsync(CSOLogHistoryViewModel model)
    {
        try
        {
            var csoLogHistory = await _dbContext.CSOLogHistories.FirstOrDefaultAsync(i => i.CSOLogId == model.CSOLogId);

            csoLogHistory.CloseBy = model.CloseBy;
            csoLogHistory.CloseOn = model.CloseOn;

            var result = await base.CreateAsync<CSOLogHistory>(csoLogHistory);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
