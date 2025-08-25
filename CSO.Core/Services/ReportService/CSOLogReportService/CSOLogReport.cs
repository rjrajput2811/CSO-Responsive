using CSO.Core.Models;
using CSO.Core.Services.SystemLogs;
using Dapper;
using System.Data;

namespace CSO.Core.Services.ReportService.CSOLogReportService;

public class CSOLogReport : ICSOLogReport
{
    private readonly IDbConnection _dbConnection;
    private readonly ISystemLogService _systemLogService;

    public CSOLogReport(IDbConnection dbConnection,
                        ISystemLogService systemLogService)
    {
        _dbConnection = dbConnection;
        _systemLogService = systemLogService;
    }

    public async Task<List<CSOLogReportGridModel>> GetCSOLogReportAsync(int userId, string financialYear)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FinYear", financialYear);
            parameters.Add("@UserId", userId);

            var result = await _dbConnection.QueryAsync<CSOLogReportGridModel>("sp_Get_CSOLogReport", parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
