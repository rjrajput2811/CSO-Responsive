using CSO.Core.Models;

namespace CSO.Core.Repositories.CSOLogRepo;

public interface ICSOLogRepository
{
    Task<List<CSOLogGridModel>> GetCSOLogListAsync();
    Task<OperationResult> CreateCSOLogAsync(CSOLogViewModel model);
    Task<OperationResult> UpdateCSOLogAsync(CSOLogViewModel model);
    Task<OperationResult> DeleteCSOLogAsync(int id);
    Task<CSOLogViewModel> GetCSOLogById(int id);

}
