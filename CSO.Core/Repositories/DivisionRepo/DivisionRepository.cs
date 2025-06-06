using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.DivisionRepo;

public class DivisionRepository : SqlTableRepository, IDivisionRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public DivisionRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<DivisionViewModel>> GetDivisionList()
    {
        try
        {
            var list = await _dbContext.Divisions
                .Select(x => new DivisionViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
