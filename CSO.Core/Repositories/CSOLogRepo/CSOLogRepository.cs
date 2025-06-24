using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CSO.Core.Repositories.CSOLogRepo;

public class CSOLogRepository : SqlTableRepository, ICSOLogRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    private readonly IDbConnection _dbConnection;
    public CSOLogRepository(CSOResponsiveDbContext dbContext,
                            ISystemLogService systemLogService,
                            IDbConnection dbConnection) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
        _dbConnection = dbConnection;
    }

    public async Task<List<CSOLogGridModel>> GetCSOLogListAsync()
    {
        try
        {
            var result = await _dbConnection.QueryAsync<CSOLogViewModel>("sp_Get_CSOLogs_Details", commandType: CommandType.StoredProcedure);

            // Map results to ViewModel
            var csoLogList = result.Select(data => new CSOLogGridModel
            {
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
                Status = Enum.IsDefined(typeof(Status), data.Status1) ? ((Status)data.Status1).ToString() : ""
                
            }).ToList();

            return csoLogList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateCSOLogAsync(CSOLogViewModel model)
    {
        try
        {
            var csoLogData = new CSOLog
            {
                UserId = model.UserId,
                Logdate = DateTime.Now,
                CategoryId = model.CategoryId,
                ComplaintTypeId = 1,
                Description = model.Description,
                SourceofComplaint = model.SourceofComplaint,
                CSOClassId = 1,
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
                CorrectiveActionDescription = "",
                MonitoringofCorrectiveActionDescription = "",
                PreventiveActionDescription = "",
                RootCauseAnalysisDescription = "",
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

    public async Task<OperationResult> UpdateCSOLogAsync(CSOLogViewModel model)
    {
        try
        {
            var csoLogData = await base.GetByIdAsync<CSOLog>(model.Id);

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
            csoLogData.CatReference = model.CatReference;
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

    public async Task<OperationResult> DeleteCSOLogAsync(int id)
    {
        try
        {
            var result = await base.DeleteAsync<CSOLog>(id);
            return result;
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
            };
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }


}
