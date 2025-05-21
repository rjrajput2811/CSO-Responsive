using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.ComplaintTypeRepo;

public class ComplaintTypeRepository : SqlTableRepository, IComplaintTypeRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public ComplaintTypeRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
