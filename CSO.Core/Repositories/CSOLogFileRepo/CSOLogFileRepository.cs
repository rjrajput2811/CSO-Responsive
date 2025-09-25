using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<CSOLogFileViewModel>> GetCSOLogFilesAsync(int csoLogId, string folderName, int? logType)
    {
        try
        {
            var query = _dbContext.CSOLogFiles
                .Where(i => i.CSOLogId == csoLogId)
                .AsQueryable();

            if (logType.HasValue)
            {
                query = query.Where(i => i.Type == logType);
            }

            var fileData = await query
                .Select(x => new CSOLogFileViewModel
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                })
                .ToListAsync();

            foreach(var file in fileData)
            {
                file.IsFileFound = IsFileFound(file.FileName, folderName);
            }

            return fileData;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    private bool IsFileFound(string image, string folderName)
    {
        var fullPath = Path.Combine(folderName, image);
        if (File.Exists(fullPath))
        {
            return true;
        }
        else
        {
            return false;
        }  
    }
}
