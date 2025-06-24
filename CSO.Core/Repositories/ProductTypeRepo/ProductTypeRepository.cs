using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.ProductTypeRepo;

public class ProductTypeRepository : SqlTableRepository, IProductTypeRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public ProductTypeRepository(CSOResponsiveDbContext dbContext,
                                  ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ProductTypeViewModel>> GetProductTypeListByBrandIdAsync(int brandId)
    {
        try
        {
            var productTypeList = await _dbContext.ProductTypes
                .Where(i => (i.BrandId ?? "").Contains(brandId.ToString()))
                .Select(x => new ProductTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return productTypeList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<ProductTypeViewModel>> GetProductTypeListByBrandAndUserAsync(int brandId, int userId)
    {
        try
        {
            // Get user's assigned ProductTypeId string, e.g., "1,2,3"
            var userAssignedProductTypes = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => x.ProductTypeId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(userAssignedProductTypes))
                return new List<ProductTypeViewModel>();

            // Parse string to List<int>
            var ProductTypeIdList = userAssignedProductTypes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id, out var value) ? value : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            // Early return if no valid IDs
            if (ProductTypeIdList.Count == 0)
                return new List<ProductTypeViewModel>();

            string brandIdWrapped = $",{brandId},";

            // Fetch all needed ProductTypes first (no filtering in SQL)
            var allProductTypes = await _dbContext.ProductTypes
                .Where(i => ("," + (i.BrandId ?? "") + ",").Contains(brandIdWrapped))
                .Select(x => new ProductTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            // Filter in-memory (LINQ to Objects)
            var list = allProductTypes
                .Where(x => ProductTypeIdList.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToList();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<ProductTypeViewModel>> GetProdTypeList()
    {
        try
        {
            //var list = await _dbContext.ProductTypes
            //    .Select(x => new ProductTypeViewModel
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        BrandId = x.BrandId,
            //        AddedOn = x.AddedOn,
            //        AddedBy = x.AddedBy,
            //        UpdatedOn = x.UpdatedOn,
            //        UpdatedBy = x.UpdatedBy
            //    })
            //    .ToListAsync();

            //return list;

            var productTypes = await _dbContext.ProductTypes.ToListAsync();
            var brands = await _dbContext.Brands.ToListAsync();
            var divisions = await _dbContext.Divisions.ToListAsync();

            var list = productTypes.Select(pt =>
            {
                var brandIds = pt.BrandId?.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                          .Select(int.Parse)
                                          .ToList() ?? new List<int>();

                var divisionNames = new List<string>();
                var brandNames = new List<string>();

                foreach (var brandId in brandIds)
                {
                    var brand = brands.FirstOrDefault(b => b.Id == brandId);
                    if (brand != null)
                    {
                        brandNames.Add(brand.Name);  // Collect brand name

                        var divIds = brand.DivisionId?.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(int.Parse)
                                                      .ToList() ?? new List<int>();

                        foreach (var divId in divIds)
                        {
                            var division = divisions.FirstOrDefault(d => d.Id == divId);
                            if (division != null)
                            {
                                divisionNames.Add(division.Name);
                            }
                        }
                    }
                }

                return new ProductTypeViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    BrandId = pt.BrandId,
                    BrandName = string.Join(", ", brandNames.Distinct()),
                    DivisionName = string.Join(", ", divisionNames.Distinct()),
                    AddedOn = pt.AddedOn,
                    AddedBy = pt.AddedBy,
                    UpdatedOn = pt.UpdatedOn,
                    UpdatedBy = pt.UpdatedBy
                };
            })
                .OrderBy(x => x.Name)
                .ToList();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<ProductType?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<ProductType>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(ProductType productType, bool returnCreatedRecord = false)
    {
        try
        {
            var productTypeToCreate = new ProductType();
            productTypeToCreate.Name = productType.Name;
            productTypeToCreate.BrandId = productType.BrandId;
            productTypeToCreate.AddedBy = productType.AddedBy;
            productTypeToCreate.AddedOn = productType.AddedOn;
            return await base.CreateAsync<ProductType>(productTypeToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(ProductType productType, bool returnUpdatedRecord = false)
    {
        try
        {
            var productTypeToCreate = await base.GetByIdAsync<ProductType>(productType.Id);
            productTypeToCreate.Name = productType.Name;
            productTypeToCreate.BrandId = productType.BrandId;
            productTypeToCreate.UpdatedBy = productType.UpdatedBy;
            productTypeToCreate.UpdatedOn = productType.UpdatedOn;
            return await base.UpdateAsync<ProductType>(productTypeToCreate, returnUpdatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteAsync(int Id)
    {
        try
        {
            return await base.DeleteAsync<ProductType>(Id);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> CheckDuplicate(string searchText, int Id)
    {
        try
        {
            bool existingflag = false;
            int? existingId = null;

            IQueryable<int> query = _dbContext.ProductTypes
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.ProductTypes
                    .Where(x =>
                           x.Name == searchText
                           && x.Id != Id)
                    .Select(x => x.Id);
            }


            existingId = await query.FirstOrDefaultAsync();

            if (existingId != null && existingId > 0)
            {
                existingflag = true;
            }

            return existingflag;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
