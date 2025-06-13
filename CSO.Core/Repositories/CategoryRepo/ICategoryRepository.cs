using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.CategoryRepo;

public interface ICategoryRepository
{
    Task<List<CategorysViewModel>> GetCategorysListAsync();
    Task<Categorys?> GetByIdAsync(int divId);
    Task<OperationResult> CreateAsync(Categorys categorys, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(Categorys categorys, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int divId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
