using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.NearestPlantRepo;

public class NearestPlantRepository : SqlTableRepository, INearestPlantRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public NearestPlantRepository(CSOResponsiveDbContext dbContext,
                                  ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantIdAsync(int plantId)
    {
        try
        {
            var nearestPlantList = await _dbContext.NearestPlants
                .Where(i => (i.PlantId ?? "").Contains(plantId.ToString()))
                .Select(x => new NearestPlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return nearestPlantList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
