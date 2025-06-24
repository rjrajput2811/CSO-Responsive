using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;

namespace CSO.Core.Repositories.NearestPlantRepo;

public class NearestPlantRepository : SqlTableRepository, INearestPlantRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public NearestPlantRepository(CSOResponsiveDbContext dbContext,
                                  ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantIdAsync(int plantId)
    {
        try
        {
            var nearestPlantList = await _dbContext.NearestPlants
                .Where(i => (i.PlantId ?? "").Contains(plantId.ToString()))
                .Select(x => new NearestPlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return nearestPlantList;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantAndUserAsync(int plantId, int userId)
    {
        try
        {
            // Get user's assigned NearestPlantId string, e.g., "1,2,3"
            var userAssignedNearestPlants = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => x.NearestPlantId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(userAssignedNearestPlants))
                return new List<NearestPlantViewModel>();

            // Parse string to List<int>
            var NearestPlantIdList = userAssignedNearestPlants
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id, out var value) ? value : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            // Early return if no valid IDs
            if (NearestPlantIdList.Count == 0)
                return new List<NearestPlantViewModel>();

            string plantIdWrapped = $",{plantId},";

            // Fetch all needed NearestPlants first (no filtering in SQL)
            var allNearestPlants = await _dbContext.NearestPlants
                .Where(i => ("," + (i.PlantId ?? "") + ",").Contains(plantIdWrapped))
                .Select(x => new NearestPlantViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            // Filter in-memory (LINQ to Objects)
            var list = allNearestPlants
                .Where(x => NearestPlantIdList.Contains(x.Id))
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

    public async Task<List<NearestPlantViewModel>> GetNearestPlantList()
    {
        try
        {
            var plants = await _dbContext.Plants.ToListAsync();
            //var divisions = await _dbContext.Divisions.ToListAsync();

            var nearestPlant = await _dbContext.NearestPlants.ToListAsync();

            var list = nearestPlant.Select(b =>
            {
                var plantIds = b.PlantId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(id => Convert.ToInt32(id))
                                               .ToList();

                // find matching division names
                var plantNames = plants
                    .Where(d => plantIds.Contains(d.Id))
                    .Select(d => d.Name)
                    .ToList();

                return new NearestPlantViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    PlantId = b.PlantId,
                    PlantName = string.Join(", ", plantNames),
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

    public async Task<NearestPlant?> GetByIdAsync(int Id)
    {
        try
        {
            var result = await base.GetByIdAsync<NearestPlant>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateAsync(NearestPlant plant, bool returnCreatedRecord = false)
    {
        try
        {
            var plantToCreate = new NearestPlant();
            plantToCreate.Name = plant.Name;
            plantToCreate.PlantId = plant.PlantId;
            plantToCreate.AddedBy = plant.AddedBy;
            plantToCreate.AddedOn = plant.AddedOn;
            return await base.CreateAsync<NearestPlant>(plantToCreate, returnCreatedRecord);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(NearestPlant plant, bool returnUpdatedRecord = false)
    {
        try
        {
            var plantToCreate = await base.GetByIdAsync<NearestPlant>(plant.Id);
            plantToCreate.Name = plant.Name;
            plantToCreate.PlantId = plant.PlantId;
            plantToCreate.UpdatedBy = plant.UpdatedBy;
            plantToCreate.UpdatedOn = plant.UpdatedOn;
            return await base.UpdateAsync<NearestPlant>(plantToCreate, returnUpdatedRecord);
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
            return await base.DeleteAsync<NearestPlant>(Id);
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

            IQueryable<int> query = _dbContext.NearestPlants
                .Where(x => x.Name == searchText)
                .Select(x => x.Id);

            // Add additional condition if Id is not 0
            if (Id != 0)
            {
                query = _dbContext.NearestPlants
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
