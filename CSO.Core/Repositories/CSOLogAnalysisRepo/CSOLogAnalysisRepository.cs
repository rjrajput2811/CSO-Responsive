using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Repositories.CSOLogAnalysisRepo
{
    public class CSOLogAnalysisRepository : SqlTableRepository, ICSOLogAnalysisRepository
    {
        private new readonly CSOResponsiveDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly IDbConnection _dbConnection;
        public CSOLogAnalysisRepository(CSOResponsiveDbContext dbContext,
                                ISystemLogService systemLogService,
                                IDbConnection dbConnection) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
            _dbConnection = dbConnection;
        }

        public async Task<List<CSOLogGridModel>> GetCSOLogListAsync(string fYear)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FinYear", fYear);

                var result = await _dbConnection.QueryAsync<CSOLogGridModel>("sp_Get_CSOLogAnal_Details", parameters, commandType: CommandType.StoredProcedure);

                // Map results to ViewModel
                var csoLogList = result.Select(data => new CSOLogGridModel
                {
                    Date = data.Logdate.ToString("dd-MM-yyyy"),
                    Id = data.Id,
                    Logdate = data.Logdate,
                    CSONo = 100 + data.Id,
                    UserName = data.UserName,
                    CSONoFYear = (100 + data.Id).ToString() + '/' + data.FinancialYear.ToString(),
                    PlantName = data.PlantName,
                    BrandName = data.BrandName,
                    ProductTypeName = data.ProductTypeName,
                    ComplainTypeName = data.ComplainTypeName,
                    Description = data.Description,
                    PendingDays = data.PendingDays,
                    DaysCompleted = 45 - (data.Logdate.AddDays(45).Date - DateTime.Now.Date).Days < 0 ? 45 + Math.Abs((data.Logdate.AddDays(45).Date - DateTime.Now.Date).Days) : (data.Logdate.AddDays(45).Date - DateTime.Now.Date).Days,
                    Status = Enum.IsDefined(typeof(Status), data.Status1) ? ((Status)data.Status1).ToString() : "",
                    RootStatus = data.RootStatus,
                    MonitorStatus = data.MonitorStatus,
                    ApprovalStatus = data.ApprovalStatus,
                    ClosureStatus = data.ClosureStatus                    

                }).ToList();

                return csoLogList;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
