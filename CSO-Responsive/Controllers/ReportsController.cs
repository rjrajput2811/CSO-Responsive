using CSO.Core.Services.ReportService.CSOLogReportService;
using CSO.Core.Services.ReportService.MSIReportService;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers;

public class ReportsController : BaseController
{
    private readonly ICSOLogReport _csoLogReport;
    private readonly IMSIReport _msiReport;

    public ReportsController(ICSOLogReport csoLogReport,
                             IMSIReport msiReport)
    {
        _csoLogReport = csoLogReport;
        _msiReport = msiReport;
    }

    public IActionResult MIS()
    {
        return View();
    }
    public IActionResult CSOLogReport()
    {
        return View();
    }

    public async Task<ActionResult> GetCSOLogReportAsync(string financialYear)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        
        var response = await _csoLogReport.GetCSOLogReportAsync(userId, financialYear);
        return Json(response);
    }

    public async Task<ActionResult> GetMSIReportByDivisionAsync(int financialYear, int month)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var response = await _msiReport.GetMSIReportByDivisionAsync(userId, financialYear, month);
        return Json(response);
    }

    public async Task<ActionResult> GetMSIReportByPlantAsync(int financialYear, int month)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var response = await _msiReport.GetMSIReportByPlantAsync(userId, financialYear, month);
        return Json(response);
    }

    public async Task<ActionResult> GetMSIReportByProductsAsync(int financialYear, int month)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var response = await _msiReport.GetMSIReportByProductsAsync(userId, financialYear, month);
        return Json(response);
    }
}
