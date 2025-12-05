using CSO.Core.Models;
using CSO.Core.Services.SystemLogs;
using Dapper;
using System.Data;

namespace CSO.Core.Services.DashboardService;

public class DashboardService : IDashboardService
{
    private readonly IDbConnection _dbConnection;
    private readonly ISystemLogService _systemLogService;

    public DashboardService(IDbConnection dbConnection,
                        ISystemLogService systemLogService)
    {
        _dbConnection = dbConnection;
        _systemLogService = systemLogService;
    }

    public async Task<DashboardViewModel?> GetDashboardDataAsync(int fYear, int userId, string divisionIds, string plantIds, DateTime? fromDate, DateTime? toDate)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@financialYear", fYear);
            parameters.Add("@UserId", userId);
            parameters.Add("@DivisionIds", divisionIds);
            parameters.Add("@PlantIds", plantIds);
            parameters.Add("@fromDate", fromDate);
            parameters.Add("@toDate", toDate);
            using var multi = await _dbConnection.QueryMultipleAsync("SP_Get_CSO_Dashboard_Data", parameters, commandType: CommandType.StoredProcedure);
            var result = await multi.ReadFirstOrDefaultAsync<DashboardViewModel>();
            result.PlantData = (await multi.ReadAsync<PlantData>()).ToList();
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
