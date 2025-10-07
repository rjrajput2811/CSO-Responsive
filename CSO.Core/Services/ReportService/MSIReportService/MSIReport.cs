using CSO.Core.Models;
using CSO.Core.Services.SystemLogs;
using Dapper;
using System.Data;

namespace CSO.Core.Services.ReportService.MSIReportService;

public class MSIReport : IMSIReport
{
    private readonly IDbConnection _dbConnection;
    private readonly ISystemLogService _systemLogService;

    public MSIReport(IDbConnection dbConnection,
                        ISystemLogService systemLogService)
    {
        _dbConnection = dbConnection;
        _systemLogService = systemLogService;
    }

    public async Task<List<MSIReportByDivisionViewModel>> GetMSIReportByDivisionAsync(int userId, int financialYear, int month)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@FYear", financialYear);
            parameters.Add("@Month", month);

            var result = await _dbConnection.QueryAsync<MSIReportByDivisionViewModel>("SP_Get_MSI_Data_By_Division", parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<MSIReportByPlantViewModel>> GetMSIReportByPlantAsync(int userId, int financialYear, int month)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@FYear", financialYear);
            parameters.Add("@Month", month);

            var result = await _dbConnection.QueryAsync<MSIReportByPlantViewModel>("SP_Get_MSI_Data_By_Plants", parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<MSIReportByProductsViewModel>> GetMSIReportByProductsAsync(int userId, int financialYear, int month)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@FYear", financialYear);
            parameters.Add("@Month", month);

            var result = await _dbConnection.QueryAsync<MSIReportByProductsViewModel>("SP_Get_MSI_Data_By_Products", parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
