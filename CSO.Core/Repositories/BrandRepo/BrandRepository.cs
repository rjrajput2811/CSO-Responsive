using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.BrandRepo;

public class BrandRepository : SqlTableRepository, IBrandRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public BrandRepository(CSOResponsiveDbContext dbContext,
                           ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<BrandViewModel>> GetBrandListByDivisionIdAsync(int divisionId)
    {
        try
        {
            var brandList = await _dbContext.Brands
                .Where(i => (i.DivisionId ?? "").Contains(divisionId.ToString()) && i.ActiveInactive == "Active")
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return brandList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<BrandViewModel>> GetBrandListByDivisionAndUserAsync(int divisionId, int userId)
    {
        try
        {
            // Get user's assigned BrandId string, e.g., "1,2,3"
            var userAssignedBrands = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => x.BrandId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(userAssignedBrands))
                return new List<BrandViewModel>();

            // Parse string to List<int>
            var BrandIdList = userAssignedBrands
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id, out var value) ? value : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            // Early return if no valid IDs
            if (BrandIdList.Count == 0)
                return new List<BrandViewModel>();

            string divisionIdWrapped = $",{divisionId},";

            // Fetch all needed Brands first by Division (no filtering in SQL)
            var allBrands = await _dbContext.Brands
                .Where(i => ("," + (i.DivisionId ?? "") + ",").Contains(divisionIdWrapped))
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            // Filter in-memory (LINQ to Objects)
            var list = allBrands
                .Where(x => BrandIdList.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToList();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<BrandViewModel?> GetBrandByIdAsync(int brandId)
    {
        try
        {
            var result = await _dbContext.Brands
                .Where(i => i.Id == brandId && i.ActiveInactive == "Active")
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<BrandViewModel>> GetBrandList()
    {
        try
        {
            var divisions = await _dbContext.Divisions.ToListAsync();

            var brands = await _dbContext.Brands.Where(i => i.ActiveInactive == "Active").ToListAsync();

            var list = brands.Select(b =>
            {
                var divisionIds = b.DivisionId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(id => Convert.ToInt32(id))
                                               .ToList();

                // find matching division names
                var divisionNames = divisions
                    .Where(d => divisionIds.Contains(d.Id))
                    .Select(d => d.Name)
                    .ToList();

                return new BrandViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    DivisionId = b.DivisionId,
                    DivisionName = string.Join(", ", divisionNames),
                    ActiveInactive = b.ActiveInactive,
                    AddedOn = b.AddedOn,
                    AddedBy = b.AddedBy,
                    UpdatedOn = b.UpdatedOn,
                    UpdatedBy = b.UpdatedBy
                };
            })
                .OrderBy(x => x.Name)
                .ToList();

            return list;

        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<BrandViewModel>> GetDrpBrandList()
    {
        try
        {
            var list = await _dbContext.Brands
                .Where(i => i.ActiveInactive == "Active")
                .Select(x => new BrandViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<Brand?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<Brand>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(Brand brand, bool returnCreatedRecord = false)
    {
        try
        {
            var brandToCreate = new Brand();
            brandToCreate.Name = brand.Name;
            brandToCreate.DivisionId = brand.DivisionId;
            brandToCreate.ActiveInactive = brand.ActiveInactive;
            brandToCreate.AddedBy = brand.AddedBy;
            brandToCreate.AddedOn = brand.AddedOn;
            return await base.CreateAsync<Brand>(brandToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(Brand brand, bool returnUpdatedRecord = false)
    {
        try
        {
            var brandToCreate = await base.GetByIdAsync<Brand>(brand.Id);
            brandToCreate.Name = brand.Name;
            brandToCreate.DivisionId = brand.DivisionId;
            brandToCreate.ActiveInactive = brand.ActiveInactive;
            brandToCreate.UpdatedBy = brand.UpdatedBy;
            brandToCreate.UpdatedOn = brand.UpdatedOn;
            return await base.UpdateAsync<Brand>(brandToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<Brand>(Id);
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

            IQueryable<int> query = _dbContext.Brands
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.Brands
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
