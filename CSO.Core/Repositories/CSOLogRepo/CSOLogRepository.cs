using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.CSOLogRepo;

public class CSOLogRepository : SqlTableRepository, ICSOLogRepository
{
    private readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CSOLogRepository(CSOResponsiveDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<CSOLogViewModel>> GetCSOListAsync()
    {
        try
        {
           

            var result = await _dbContext.CSOLogs.FromSqlRaw("EXEC sp_Get_CSOLogs_Details").ToListAsync();

            // Map results to ViewModel
            var viewModelList = result.Select(data => new CSOLogViewModel
            {
                Id = data.Id,
                Logdate = data.Logdate,
                CSONo =  100 + data.Id,
                UserName = data.UserName,
                CSONoFYear = (100 + data.Id).ToString() + '/' + data.FinancialYear.ToString(),
                PlantName = data.PlantName,
                BrandName = data.BrandName ,
                ProductTypeName = data.ProductTypeName,
                ComplainTypeName = data.ComplainTypeName,
                Description = data.Description
            }).ToList();

            return viewModelList;

        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }


}
