using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.ProductTypeRepo;

public interface IProductTypeRepository
{
    Task<List<ProductTypeViewModel>> GetProductTypeListByBrandIdAsync(int brandId);
    Task<List<ProductTypeViewModel>> GetProductTypeListByBrandAndUserAsync(int brandId, int userId);
    Task<List<ProductTypeViewModel>> GetProdTypeList();
    Task<ProductType?> GetByIdAsync(int productId);
    Task<OperationResult> CreateAsync(ProductType productType, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(ProductType productType, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int productId);
    Task<bool> CheckDuplicate(string searchText, int Id);

}
