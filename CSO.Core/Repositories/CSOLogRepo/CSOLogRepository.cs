using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.CSOLogRepo;

public class CSOLogRepository : SqlTableRepository, ICSOLogRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CSOLogRepository(CSOResponsiveDbContext dbContext,
                            ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<CSOLogGridModel>> GetCSOLogListAsync()
    {
        try
        {
            var result = await _dbContext.CSOLogs.FromSqlRaw("EXEC sp_Get_CSOLogs_Details").ToListAsync();

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
                ComplaintTypeName = data.ComplainTypeName,
                Description = data.Description
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
                SKUDetails = model.SKUDetails
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

    public Task<CSOLogViewModel> GetCSOLogById(int id)
    {
        throw new NotImplementedException();
    }


}
