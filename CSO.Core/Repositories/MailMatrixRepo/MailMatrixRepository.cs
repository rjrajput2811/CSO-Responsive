using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.MailMatrixRepo;

public class MailMatrixRepository : SqlTableRepository, IMailMatrixRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public MailMatrixRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
