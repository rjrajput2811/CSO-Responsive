using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.ProductTypeRepo;

public class ProductTypeRepository : SqlTableRepository, IProductTypeRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public ProductTypeRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
