using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.CategoryRepo;

public class CategoryRepository : SqlTableRepository, ICategoryRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public CategoryRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
