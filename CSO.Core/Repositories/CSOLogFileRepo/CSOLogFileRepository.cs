using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSO.Core.Repositories.CSOLogFileRepo;

public class CSOLogFileRepository : SqlTableRepository, ICSOLogFileRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CSOLogFileRepository(CSOResponsiveDbContext dbContext,
                                ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult> InsertCSOLogFileInfoAsync(CSOLogFileViewModel model)
    {
        try
        {
            var csoLogFileData = new CSOLogFile
            {
                FilePath = model.FilePath,
                FileName = model.FileName,
                Type = model.Type,
                CSOLogId = model.CSOLogId,
                AddedBy = model.AddedBy,
                AddedOn = model.AddedOn
            };

            var result = await base.CreateAsync<CSOLogFile>(csoLogFileData);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
