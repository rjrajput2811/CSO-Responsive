using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.PlantRepo;

public class PlantRepository : SqlTableRepository, IPlantRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public PlantRepository(CSOResponsiveDbContext dbContext,
                           ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<PlantViewModel>> GetPlantListByDivisionIdAsync(int divisionId)
    {
        try
        {
            var plantList = await _dbContext.Plants
                .Where(i => (i.DivisionId ?? "").Contains(divisionId.ToString()))
                .Select(x => new PlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return plantList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<PlantViewModel>> GetPlantListByDivisionAndUserAsync(int divisionId, int userId)
    {
        try
        {
            // Get user's assigned PlantId string, e.g., "1,2,3"
            var userAssignedPlants = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => x.PlantId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(userAssignedPlants))
                return new List<PlantViewModel>();

            // Parse string to List<int>
            var PlantIdList = userAssignedPlants
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id, out var value) ? value : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            // Early return if no valid IDs
            if (PlantIdList.Count == 0)
                return new List<PlantViewModel>();

            string divisionIdWrapped = $",{divisionId},";

            // Fetch all needed Plants first (no filtering in SQL)
            var allPlants = await _dbContext.Plants
                .Where(i => ("," + (i.DivisionId ?? "") + ",").Contains(divisionIdWrapped))
                .Select(x => new PlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            // Filter in-memory (LINQ to Objects)
            var list = allPlants
                .Where(x => PlantIdList.Contains(x.Id))
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

    public async Task<List<PlantViewModel>> GetPlantList()
    {
        try
        {
            var divisions = await _dbContext.Divisions.ToListAsync();

            var plant = await _dbContext.Plants.ToListAsync();

            var list = plant.Select(b =>
            {
                var divisionIds = b.DivisionId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(id => Convert.ToInt32(id))
                                               .ToList();

                // find matching division names
                var divisionNames = divisions
                    .Where(d => divisionIds.Contains(d.Id))
                    .Select(d => d.Name)
                    .ToList();

                return new PlantViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    DivisionId = b.DivisionId,
                    DivisionName = string.Join(", ", divisionNames),
                    IsThirdParty = b.IsThirdParty,
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

    public async Task<List<PlantViewModel>> GetDrpPlantList()
    {
        try
        {
            var list = await _dbContext.Plants
                .Select(x => new PlantViewModel
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

    public async Task<Plant?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<Plant>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(Plant plant, bool returnCreatedRecord = false)
    {
        try
        {
            var plantToCreate = new Plant();
            plantToCreate.Name = plant.Name;
            plantToCreate.DivisionId = plant.DivisionId;
            plantToCreate.IsThirdParty = plant.IsThirdParty;
            plantToCreate.AddedBy = plant.AddedBy;
            plantToCreate.AddedOn = plant.AddedOn;
            return await base.CreateAsync<Plant>(plantToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(Plant plant, bool returnUpdatedRecord = false)
    {
        try
        {
            var plantToCreate = await base.GetByIdAsync<Plant>(plant.Id);
            plantToCreate.Name = plant.Name;
            plantToCreate.DivisionId = plant.DivisionId;
            plantToCreate.IsThirdParty = plant.IsThirdParty;
            plantToCreate.UpdatedBy = plant.UpdatedBy;
            plantToCreate.UpdatedOn = plant.UpdatedOn;
            return await base.UpdateAsync<Plant>(plantToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<Plant>(Id);
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

            IQueryable<int> query = _dbContext.Plants
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.Plants
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
