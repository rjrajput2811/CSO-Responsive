using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.CSOClassRepo;

public class CSOClassRepository : SqlTableRepository, ICSOClassRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public CSOClassRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
