using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.BrandRepo;

public class BrandRepository : SqlTableRepository, IBrandRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public BrandRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    

}
