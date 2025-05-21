using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.UserRepo;

public class UserRepository : SqlTableRepository, IUserRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public UserRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
