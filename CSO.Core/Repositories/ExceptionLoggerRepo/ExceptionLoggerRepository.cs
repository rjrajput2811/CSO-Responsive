using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.ExceptionLoggerRepo;

public class ExceptionLoggerRepository : SqlTableRepository, IExceptionLoggerRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public ExceptionLoggerRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
