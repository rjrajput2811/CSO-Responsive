using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;

namespace CSO.Core.Repositories.CSOLogFileRepo;

public class CSOLogFileRepository : SqlTableRepository, ICSOLogFileRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CSOLogFileRepository(CSOResponsiveDbContext dbContext,
                                ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }
}
