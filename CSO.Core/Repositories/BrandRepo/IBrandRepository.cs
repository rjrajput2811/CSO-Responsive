using CSO.Core.Models;

namespace CSO.Core.Repositories.BrandRepo;

public interface IBrandRepository
{
    Task<List<BrandViewModel>> GetBrandListByDivisionIdAsync(int divisionId);
    Task<BrandViewModel?> GetBrandByIdAsync(int brandId);
}
