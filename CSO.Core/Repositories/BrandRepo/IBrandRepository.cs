using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.BrandRepo;

public interface IBrandRepository
{
    Task<OperationResult> CreateBrandAsync(Brand brand, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateBrandAsync(Brand brand, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteBrandAsync(int brandId);
    Task<Brand?> GetBrandByIdAsync(int brandId);
}
