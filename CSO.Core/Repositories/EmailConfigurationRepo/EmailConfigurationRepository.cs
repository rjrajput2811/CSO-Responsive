using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.EmailConfigurationRepo;

public class EmailConfigurationRepository : SqlTableRepository, IEmailConfigurationRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public EmailConfigurationRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
