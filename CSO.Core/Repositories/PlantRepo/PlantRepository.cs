using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.PlantRepo;

public class PlantRepository : SqlTableRepository, IPlantRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public PlantRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
