using CSO.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Services.ReportService.CSOLogReportService;

public interface ICSOLogReport
{
    Task<List<CSOLogReportGridModel>> GetCSOLogReportAsync(int userId, string financialYear);
}
