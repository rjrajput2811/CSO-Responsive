using CSO.Core.Services.ReportService.CSOLogReportService;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers;

public class ReportsController : BaseController
{
    private readonly ICSOLogReport _csoLogReport;

    public ReportsController(ICSOLogReport csoLogReport)
    {
        _csoLogReport = csoLogReport;
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
}
