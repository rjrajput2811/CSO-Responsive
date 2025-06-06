using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.PlantRepo;

public class PlantRepository : SqlTableRepository, IPlantRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public PlantRepository(CSOResponsiveDbContext dbContext,
                           ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<PlantViewModel>> GetPlantListByDivisionIdAsync(int divisionId)
    {
        try
        {
            var plantList = await _dbContext.Plants
                .Where(i => (i.DivisionId ?? "").Contains(divisionId.ToString()))
                .Select(x => new PlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return plantList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
