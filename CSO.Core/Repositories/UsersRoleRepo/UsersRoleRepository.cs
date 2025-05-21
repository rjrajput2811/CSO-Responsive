using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.UsersRoleRepo;

public class UsersRoleRepository : SqlTableRepository, IUsersRoleRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public UsersRoleRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
