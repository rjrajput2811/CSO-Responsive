using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Dapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CSOLogGridModel>> GetCSOLogListAsync(string fYear, int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FinYear", fYear);
                parameters.Add("@UserId", userId);

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
                    DivisionId = data.DivisionId,
                    PlantId = data.PlantId,
                    PlantName = data.PlantName,
                    BrandName = data.BrandName,
                    ProductTypeName = data.ProductTypeName,
                    ComplainTypeName = data.ComplainTypeName,
                    Description = data.Description,
                    PendingDays = data.PendingDays,
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

        public async Task<CSOLogViewModel> GetCSOLogById(int id)
        {
            try
            {
                var csoLogDetails = await base.GetByIdAsync<CSOLog>(id);
                var result = new CSOLogViewModel
                {
                    Id = csoLogDetails.Id,
                    UserId = csoLogDetails.UserId,
                    UserName = _dbContext.Users.Where(i => i.Id == csoLogDetails.UserId).Select(x => x.Name).FirstOrDefault(),
                    Logdate = csoLogDetails.Logdate,
                    CSONo = 100 + csoLogDetails.Id,
                    DivisionId = csoLogDetails.DivisionId,
                    CategoryId = csoLogDetails.CategoryId,
                    ComplaintTypeId = csoLogDetails.ComplaintTypeId,
                    Description = csoLogDetails.Description,
                    SourceofComplaint = csoLogDetails.SourceofComplaint,
                    CSOClassId = csoLogDetails.CSOClassId,
                    BrandId = csoLogDetails.BrandId,
                    ProductTypeId = csoLogDetails.ProductTypeId,
                    PlantId = csoLogDetails.PlantId,
                    NearestPlantId = csoLogDetails.NearestPlantId,
                    Batch = csoLogDetails.Batch,
                    PKDDate = csoLogDetails.PKDDate,
                    Quantity = csoLogDetails.Quantity,
                    SuppliedQuantity = csoLogDetails.SuppliedQuantity,
                    CatReference = csoLogDetails.CatReference,
                    IsSampleShipped = csoLogDetails.IsSampleShipped,
                    TrackingNo = csoLogDetails.TrackingNo,
                    Status1 = csoLogDetails.Status1,
                    Review1 = csoLogDetails.Review1,
                    Status2 = csoLogDetails.Status2,
                    Review2 = csoLogDetails.Review2,
                    AddedBy = csoLogDetails.AddedBy,
                    SKUDetails = csoLogDetails.SKUDetails,
                    FinancialYear = csoLogDetails.FinancialYear,
                    RootCauseAnalysisDescription = csoLogDetails.RootCauseAnalysisDescription,
                    PreventiveActionDescription = csoLogDetails.PreventiveActionDescription,
                    CorrectiveActionDescription = csoLogDetails.CorrectiveActionDescription,
                    MonitoringofCorrectiveActionDescription = csoLogDetails.MonitoringofCorrectiveActionDescription,
                    IsRootCauseSubmitted = csoLogDetails.IsRootCauseSubmitted,
                    IsMonitorSubmitted = csoLogDetails.IsMonitorSubmitted,
                    IsApproveSubmitted = csoLogDetails.IsApproveSubmitted,
                };

                var csoLogHistory = await _dbContext.CSOLogHistories
                    .Where(i => i.CSOLogId == id)
                    .FirstOrDefaultAsync();

                if (csoLogHistory != null)
                {
                    var rootRecycleDays = await _dbContext.RecycleDays
                        .Where(i => i.CSOLogPhase == (int)Status.RootCause && result.Logdate >= i.FromDate && result.Logdate <= i.ToDate)
                        .FirstOrDefaultAsync();

                    if(csoLogHistory.RootCauseOn.HasValue && result.IsRootCauseSubmitted)
                    {
                        result.IsRcaComplete = true;
                        result.RcaDate = csoLogHistory.RootCauseOn.Value;
                        result.RcaUserName = _dbContext.Users.Where(i => i.Id == csoLogHistory.RootCauseBy).Select(x => x.Name).FirstOrDefault();
                    }
                    else
                    {
                        result.IsRcaInProgress = true;
                        if(rootRecycleDays != null)
                        {
                            result.RcaDaysProgressed = (int)Math.Floor((DateTime.Now - result.Logdate).TotalDays);
                            result.RcaThresholdDays = rootRecycleDays.ThresholdDays;
                        }
                        else
                        {
                            result.RcaDaysProgressed = (int)Math.Floor((DateTime.Now - result.Logdate).TotalDays);
                            result.RcaThresholdDays = 45;
                        }

                        if(result.RcaDaysProgressed > result.RcaThresholdDays)
                        {
                            result.IsRcaOverdue = true;
                        }
                    }

                    if(result.IsRootCauseSubmitted)
                    {
                        var monitoringRecycleDay = await _dbContext.RecycleDays
                            .Where(i => i.CSOLogPhase == (int)Status.Monitor && result.Logdate >= i.FromDate && result.Logdate <= i.ToDate)
                            .FirstOrDefaultAsync();

                        if (csoLogHistory.MonitoringOn.HasValue && result.IsMonitorSubmitted)
                        {
                            result.IsMonitoringComplete = true;
                            result.MonitoringDate = csoLogHistory.MonitoringOn.Value;
                            result.MonitoringUserName = _dbContext.Users.Where(i => i.Id == csoLogHistory.MonitoringBy).Select(x => x.Name).FirstOrDefault();
                        }
                        else
                        {
                            result.IsMonitoringInProgress = true;
                            if (monitoringRecycleDay != null)
                            {
                                result.MonitoringDaysProgressed = (int)Math.Floor((DateTime.Now - csoLogHistory.RootCauseOn.Value).TotalDays);
                                result.MonitoringThresholdDays = monitoringRecycleDay.ThresholdDays;
                            }
                            else
                            {
                                result.MonitoringDaysProgressed = (int)Math.Floor((DateTime.Now - result.Logdate).TotalDays);
                                result.MonitoringThresholdDays = 45;
                            }

                            if (result.MonitoringDaysProgressed > result.MonitoringThresholdDays)
                            {
                                result.IsMonitoringOverdue = true;
                            }
                        }
                    }

                    if(result.IsMonitorSubmitted)
                    {
                        var reviewRecycleDay = await _dbContext.RecycleDays
                            .Where(i => i.CSOLogPhase == (int)Status.Approve && result.Logdate >= i.FromDate && result.Logdate <= i.ToDate)
                            .FirstOrDefaultAsync();

                        if (csoLogHistory.ReviewOn.HasValue && result.IsApproveSubmitted)
                        {
                            result.IsReviewComplete = true;
                            result.ReviewDate = csoLogHistory.ReviewOn.Value;
                            result.ReviewUserName = _dbContext.Users.Where(i => i.Id == csoLogHistory.ReviewBy).Select(x => x.Name).FirstOrDefault();
                        }
                        else
                        {
                            result.IsReviewInProgress = true;
                            if (reviewRecycleDay != null)
                            {
                                result.ReviewDaysProgressed = (int)Math.Floor((DateTime.Now - csoLogHistory.MonitoringOn.Value).TotalDays);
                                result.ReviewThresholdDays = reviewRecycleDay.ThresholdDays;
                            }
                            else
                            {
                                result.ReviewDaysProgressed = (int)Math.Floor((DateTime.Now - result.Logdate).TotalDays);
                                result.ReviewThresholdDays = 45;
                            }
                        }

                        if (result.ReviewDaysProgressed > result.ReviewThresholdDays)
                        {
                            result.IsReviewOverdue = true;
                        }
                    }

                    if(result.IsApproveSubmitted)
                    {
                        var closeRecycleDay = await _dbContext.RecycleDays
                            .Where(i => i.CSOLogPhase == (int)Status.Close && result.Logdate >= i.FromDate && result.Logdate <= i.ToDate)
                            .FirstOrDefaultAsync();

                        if (csoLogHistory.CloseOn.HasValue && result.IsApproveSubmitted)
                        {
                            result.IsCloseComplete = true;
                            result.CloseDate = csoLogHistory.CloseOn.Value;
                            result.CloseUserName = _dbContext.Users.Where(i => i.Id == csoLogHistory.CloseBy).Select(x => x.Name).FirstOrDefault();
                        }
                        else
                        {
                            result.IsCloseInProgress = true;
                            if (closeRecycleDay != null)
                            {
                                result.CloseDaysProgressed = (int)Math.Floor((DateTime.Now - csoLogHistory.ReviewOn.Value).TotalDays);
                                result.CloseThresholdDays = closeRecycleDay.ThresholdDays;
                            }
                            else
                            {
                                result.CloseDaysProgressed = (int)Math.Floor((DateTime.Now - result.Logdate).TotalDays);
                                result.CloseThresholdDays = 45;
                            }
                        }

                        if (result.CloseDaysProgressed > result.CloseThresholdDays)
                        {
                            result.IsCloseOverdue = true;
                        }
                    }
                }
                return result;
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
                csoLogData.IsRootCauseSubmitted = model.IsRootCauseSubmitted;
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
                csoLogData.IsMonitorSubmitted = model.IsMonitorSubmitted;
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
                    csoLogData.IsApproveSubmitted = model.IsApproveSubmitted;
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
                        csoLogData.IsMonitorSubmitted = false;
                        csoLogData.IsApproveSubmitted = false;
                    }
                    if(model.RejectRevertStatus == "monitor")
                    {
                        csoLogData.Status1 = (int)Status.Monitor;
                        csoLogData.UpdatedBy = model.UpdatedBy;
                        csoLogData.UpdatedOn = model.UpdatedOn;
                        csoLogData.IsApproveSubmitted = false;
                    }
                    if(model.RejectRevertStatus == "log")
                    {
                        csoLogData.Status1 = (int)Status.Open;
                        csoLogData.UpdatedBy = model.UpdatedBy;
                        csoLogData.UpdatedOn = model.UpdatedOn;
                        csoLogData.IsRootCauseSubmitted = false;
                        csoLogData.IsMonitorSubmitted = false;
                        csoLogData.IsApproveSubmitted = false;
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
