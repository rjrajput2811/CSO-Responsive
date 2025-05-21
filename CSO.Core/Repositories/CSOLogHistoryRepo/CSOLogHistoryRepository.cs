using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.CSOLogHistoryRepo;

public class CSOLogHistoryRepository : SqlTableRepository, ICSOLogHistoryRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public CSOLogHistoryRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
