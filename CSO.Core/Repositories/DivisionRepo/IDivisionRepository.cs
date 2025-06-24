using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using System.Numerics;

namespace CSO.Core.Repositories.DivisionRepo;

public interface IDivisionRepository
{
    Task<List<DivisionViewModel>> GetDivisionList();
    Task<List<DivisionViewModel>> GetDivisionListByUserAsync(int userId);
    Task<Division?> GetByIdAsync(int divId);
    Task<OperationResult> CreateAsync(Division division, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(Division division, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int divId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
