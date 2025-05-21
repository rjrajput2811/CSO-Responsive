using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.CSOLogRepo;

public class CSOLogRepository : SqlTableRepository, ICSOLogRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public CSOLogRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
