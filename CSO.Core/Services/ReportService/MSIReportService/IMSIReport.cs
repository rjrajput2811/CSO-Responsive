using CSO.Core.Models;

namespace CSO.Core.Services.ReportService.MSIReportService;

public interface IMSIReport
{
    Task<List<MSIReportByDivisionViewModel>> GetMSIReportByDivisionAsync(int userId, int financialYear, int month);
    Task<List<MSIReportByPlantViewModel>> GetMSIReportByPlantAsync(int userId, int financialYear, int month);
    Task<List<MSIReportByProductsViewModel>> GetMSIReportByProductsAsync(int userId, int financialYear, int month);
}
