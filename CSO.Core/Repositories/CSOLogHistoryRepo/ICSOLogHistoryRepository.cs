using CSO.Core.Models;

namespace CSO.Core.Repositories.CSOLogHistoryRepo;

public interface ICSOLogHistoryRepository
{
    Task<bool> CheckCSOLogHistryExistsAsync(int csoLogId);
    Task<OperationResult> CreateCSOLogHistoryAsync(CSOLogHistoryViewModel model);
    Task<OperationResult> UpdateCSOLogHistoryForRootCauseAsync(CSOLogHistoryViewModel model);
    Task<OperationResult> UpdateCSOLogHistoryForMonitorAsync(CSOLogHistoryViewModel model);
    Task<OperationResult> UpdateCSOLogHistoryForApproveRejectAsync(CSOLogHistoryViewModel model);
    Task<OperationResult> UpdateCSOLogHistoryForCloseAsync(CSOLogHistoryViewModel model);
}
