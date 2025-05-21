using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.BrandRepo;

public class BrandRepository : SqlTableRepository, IBrandRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public BrandRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult> CreateBrandAsync(Brand brand, bool returnCreatedRecord = false)
    {
        try
        {
            var result = await base.CreateAsync<Brand>(brand, returnCreatedRecord);
            return result; 
        }
        catch
        {
            throw;
        }
    }

    public async Task<OperationResult> UpdateBrandAsync(Brand brand, bool returnUpdatedRecord = false)
    {
        try
        {
            var result = await base.UpdateAsync<Brand>(brand, returnUpdatedRecord);
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<OperationResult> DeleteBrandAsync(int brandId)
    {
        try
        {
            var result = await base.DeleteAsync<Brand>(brandId);
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<Brand?> GetBrandByIdAsync(int brandId)
    {
        try
        {
            var result = await base.GetByIdAsync<Brand>(brandId);
            return result;
        }
        catch
        {
            throw;
        }
    }
}
