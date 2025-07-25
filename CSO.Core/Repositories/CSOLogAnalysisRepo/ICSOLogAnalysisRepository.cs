using CSO.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Repositories.CSOLogAnalysisRepo
{
    public interface ICSOLogAnalysisRepository
    {
        Task<List<CSOLogGridModel>> GetCSOLogListAsync(string fYear);
        Task<OperationResult> CreateCSOLogAnyaAsync(CSOLogViewModel model);
        Task<OperationResult> UpdateCSOLogAnyaAsync(CSOLogViewModel model);
        Task<OperationResult> UpdateCSOLogAnalysisForRootCauseAsync(CSOLogViewModel model);
        Task<OperationResult> UpdateCSOLogAnalysisForMonitorAsync(CSOLogViewModel model);
        Task<OperationResult> UpdateCSOLogAnalysisForApproveRejectAsync(CSOLogViewModel model);
        Task<OperationResult> UpdateCSOLogAnalysisForCloseAsync(CSOLogViewModel model);
    }
}
