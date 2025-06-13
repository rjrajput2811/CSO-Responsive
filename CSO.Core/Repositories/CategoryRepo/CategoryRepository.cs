using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.CategoryRepo;

public class CategoryRepository : SqlTableRepository, ICategoryRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public CategoryRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<CategorysViewModel>> GetCategorysListAsync()
    {
        try
        {
            var list = await _dbContext.Categories
                .Select(x => new CategorysViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AddedOn = x.AddedOn,
                    AddedBy = x.AddedBy,
                    UpdatedOn = x.UpdatedOn,
                    UpdatedBy = x.UpdatedBy,
                    AddedByUser = _dbContext.Users.Where(i => i.Id == x.AddedBy).Select(x => x.Name).FirstOrDefault(),
                    UpdatedByUser = _dbContext.Users.Where(i => i.Id == x.UpdatedBy).Select(x => x.Name).FirstOrDefault()
                })
                .OrderBy(o => o.Name)
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<Categorys?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<Categorys>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(Categorys categorys, bool returnCreatedRecord = false)
    {
        try
        {
            var categorysToCreate = new Categorys();
            categorysToCreate.Name = categorys.Name;
            categorysToCreate.AddedBy = categorys.AddedBy;
            categorysToCreate.AddedOn = categorys.AddedOn;
            return await base.CreateAsync<Categorys>(categorysToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(Categorys categorys, bool returnUpdatedRecord = false)
    {
        try
        {
            var categorysToCreate = await base.GetByIdAsync<Categorys>(categorys.Id);
            categorysToCreate.Name = categorys.Name;
            categorysToCreate.UpdatedBy = categorys.UpdatedBy;
            categorysToCreate.UpdatedOn = categorys.UpdatedOn;
            return await base.UpdateAsync<Categorys>(categorysToCreate, returnUpdatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteAsync(int Id)
    {
        try
        {
            return await base.DeleteAsync<Categorys>(Id);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> CheckDuplicate(string searchText, int Id)
    {
        try
        {
            bool existingflag = false;
            int? existingId = null;

            IQueryable<int> query = _dbContext.Categories
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.Categories
                    .Where(x =>
                           x.Name == searchText
                           && x.Id != Id)
                    .Select(x => x.Id);
            }


            existingId = await query.FirstOrDefaultAsync();

            if (existingId != null && existingId > 0)
            {
                existingflag = true;
            }

            return existingflag;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
