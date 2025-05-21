using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.RecycleDayRepo;

public class RecycleDayRepository : SqlTableRepository, IRecycleDayRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public RecycleDayRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
