using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.BrandRepo;

public class BrandRepository : SqlTableRepository, IBrandRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public BrandRepository(CSOResponsiveDbContext dbContext,
                           ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<BrandViewModel>> GetBrandListByDivisionIdAsync(int divisionId)
    {
        try
        {
            var brandList = await _dbContext.Brands
                .Where(i => (i.DivisionId ?? "").Contains(divisionId.ToString()))
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return brandList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<BrandViewModel?> GetBrandByIdAsync(int brandId)
    {
        try
        {
            var result = await _dbContext.Brands
                .Where(i => i.Id == brandId)
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
