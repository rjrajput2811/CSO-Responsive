using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.NearestPlantRepo;

public class NearestPlantRepository : SqlTableRepository, INearestPlantRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public NearestPlantRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
