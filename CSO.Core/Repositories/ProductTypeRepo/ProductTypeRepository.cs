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
}
