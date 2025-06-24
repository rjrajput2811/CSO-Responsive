using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.NearestPlantRepo;

public interface INearestPlantRepository
{
    Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantIdAsync(int plantId);
    Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantAndUserAsync(int plantId, int userId);
    Task<List<NearestPlantViewModel>> GetNearestPlantList();
    Task<NearestPlant?> GetByIdAsync(int nearestPlantId);
    Task<OperationResult> CreateAsync(NearestPlant nearestPlant, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(NearestPlant nearestPlant, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int nearestPlantId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
