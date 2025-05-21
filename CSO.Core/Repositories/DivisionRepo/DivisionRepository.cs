using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.DivisionRepo;

public class DivisionRepository : SqlTableRepository, IDivisionRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public DivisionRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
