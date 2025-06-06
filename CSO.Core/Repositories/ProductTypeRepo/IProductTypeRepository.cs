using CSO.Core.Models;

namespace CSO.Core.Repositories.ProductTypeRepo;

public interface IProductTypeRepository
{
    Task<List<ProductTypeViewModel>> GetProductTypeListByBrandIdAsync(int brandId);
}
