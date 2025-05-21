using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.CSOLogFileRepo;

public class CSOLogFileRepository : SqlTableRepository, ICSOLogFileRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public CSOLogFileRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
