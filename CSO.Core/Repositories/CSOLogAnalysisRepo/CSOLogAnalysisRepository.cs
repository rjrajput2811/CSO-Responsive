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

        public async Task<OperationResult> CreateCSOLogAnyaAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = new CSOLog
                {
                    UserId = model.UserId,
                    Logdate = DateTime.Now,
                    CategoryId = model.CategoryId,
                    ComplaintTypeId = model.ComplaintTypeId,
                    Description = model.Description,
                    SourceofComplaint = model.SourceofComplaint,
                    CSOClassId = model.CSOClassId,
                    DivisionId = model.DivisionId,
                    BrandId = model.BrandId,
                    ProductTypeId = model.ProductTypeId,
                    PlantId = model.PlantId,
                    NearestPlantId = model.NearestPlantId,
                    Batch = model.Batch,
                    Date = DateTime.Now,
                    PKDDate = model.PKDDate,
                    Quantity = model.Quantity,
                    SuppliedQuantity = model.SuppliedQuantity,
                    CatReference = model.CatReference,
                    IsSampleShipped = model.IsSampleShipped,
                    TrackingNo = model.IsSampleShipped == true ? model.TrackingNo : "",
                    Status1 = (int)Status.Open,
                    AddedBy = model.UserId,
                    AddedOn = DateTime.Now,
                    FinancialYear = model.FinancialYear,
                    SKUDetails = model.SKUDetails,
                    CorrectiveActionDescription = model.CorrectiveActionDescription,
                    MonitoringofCorrectiveActionDescription = model.MonitoringofCorrectiveActionDescription,
                    PreventiveActionDescription = model.PreventiveActionDescription,
                    RootCauseAnalysisDescription = model.RootCauseAnalysisDescription,
                    Review1 = "",
                    Review2 = ""
                };

                var result = await base.CreateAsync<CSOLog>(csoLogData);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateCSOLogAnyaAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

                csoLogData.ComplaintTypeId = model.ComplaintTypeId;
                csoLogData.CSOClassId = model.CSOClassId;                
                csoLogData.DivisionId = model.DivisionId;
                csoLogData.CategoryId = model.CategoryId;
                csoLogData.Description = model.Description;
                csoLogData.SourceofComplaint = model.SourceofComplaint;
                csoLogData.BrandId = model.BrandId;
                csoLogData.ProductTypeId = model.ProductTypeId;
                csoLogData.PlantId = model.PlantId;
                csoLogData.NearestPlantId = model.NearestPlantId;
                csoLogData.Batch = model.Batch;
                csoLogData.PKDDate = model.PKDDate;
                csoLogData.Quantity = model.Quantity;
                csoLogData.SuppliedQuantity = model.SuppliedQuantity;
                csoLogData.CatReference = model.CatReference ?? "";
                csoLogData.IsSampleShipped = model.IsSampleShipped;
                csoLogData.TrackingNo = model.IsSampleShipped == true ? model.TrackingNo : "";
                csoLogData.UpdatedBy = model.UserId;
                csoLogData.UpdatedOn = DateTime.Now;
                csoLogData.SKUDetails = model.SKUDetails;

                var result = await base.UpdateAsync<CSOLog>(csoLogData);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateCSOLogAnalysisForRootCauseAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

                csoLogData.Status1 = model.Status1;
                csoLogData.RootCauseAnalysisDescription = model.RootCauseAnalysisDescription;
                csoLogData.CorrectiveActionDescription = model.CorrectiveActionDescription;
                csoLogData.PreventiveActionDescription = model.PreventiveActionDescription;
                csoLogData.UpdatedBy = model.UpdatedBy;
                csoLogData.UpdatedOn = model.UpdatedOn;

                var result = await base.UpdateAsync<CSOLog>(csoLogData);

                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateCSOLogAnalysisForMonitorAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

                csoLogData.Status1 = model.Status1;
                csoLogData.MonitoringofCorrectiveActionDescription = model.MonitoringofCorrectiveActionDescription;
                csoLogData.UpdatedBy = model.UpdatedBy;
                csoLogData.UpdatedOn = model.UpdatedOn;

                var result = await base.UpdateAsync<CSOLog>(csoLogData);

                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateCSOLogAnalysisForApproveRejectAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

                if (model.Status1 == 4)
                {
                    csoLogData.Status1 = (int)Status.Approve;
                    csoLogData.Review1 = model.Review1;
                    csoLogData.UpdatedBy = model.UpdatedBy;
                    csoLogData.UpdatedOn = model.UpdatedOn;
                }
                else
                {
                    if(model.RejectRevertStatus == "root")
                    {
                        csoLogData.Status1 = (int)Status.RootCause;
                        csoLogData.UpdatedBy = model.UpdatedBy;
                        csoLogData.UpdatedOn = model.UpdatedOn;
                    }
                    if(model.RejectRevertStatus == "monitor")
                    {
                        csoLogData.Status1 = (int)Status.Monitor;
                        csoLogData.UpdatedBy = model.UpdatedBy;
                        csoLogData.UpdatedOn = model.UpdatedOn;
                    }
                    if(model.RejectRevertStatus == "log")
                    {
                        csoLogData.Status1 = (int)Status.Open;
                        csoLogData.UpdatedBy = model.UpdatedBy;
                        csoLogData.UpdatedOn = model.UpdatedOn;
                    }
                }

                var result = await base.UpdateAsync<CSOLog>(csoLogData);

                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateCSOLogAnalysisForCloseAsync(CSOLogViewModel model)
        {
            try
            {
                var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

                csoLogData.Status1 = model.Status1;
                csoLogData.Review2 = model.Review2;
                csoLogData.UpdatedBy = model.UpdatedBy;
                csoLogData.UpdatedOn = model.UpdatedOn;

                var result = await base.UpdateAsync<CSOLog>(csoLogData);

                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
