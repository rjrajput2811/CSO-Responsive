using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.BrandRepo;

public interface IBrandRepository
{
    Task<List<BrandViewModel>> GetBrandListByDivisionIdAsync(int divisionId);
    Task<List<BrandViewModel>> GetBrandListByDivisionAndUserAsync(int divisionId, int userId);
    Task<BrandViewModel?> GetBrandByIdAsync(int brandId);
    Task<List<BrandViewModel>> GetBrandList();
    Task<List<BrandViewModel>> GetDrpBrandList();
    Task<Brand?> GetByIdAsync(int brandId);
    Task<OperationResult> CreateAsync(Brand brand, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(Brand brand, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int brandId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
