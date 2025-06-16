using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.CSOClassRepo;

public interface ICSOClassRepository 
{
    Task<List<CSOClassViewModel>> GetCsoClassList();
    Task<CSOClass?> GetByIdAsync(int csoClassId);
    Task<OperationResult> CreateAsync(CSOClass csoClass, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(CSOClass csoClass, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int csoClassId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
