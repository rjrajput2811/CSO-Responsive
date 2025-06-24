using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.PlantRepo;

public interface IPlantRepository
{
    Task<List<PlantViewModel>> GetPlantListByDivisionIdAsync(int divisionId);
    Task<List<PlantViewModel>> GetPlantListByDivisionAndUserAsync(int divisionId, int userId);
    Task<List<PlantViewModel>> GetPlantList();
    Task<List<PlantViewModel>> GetDrpPlantList();
    Task<Plant?> GetByIdAsync(int plantId);
    Task<OperationResult> CreateAsync(Plant plant, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(Plant plant, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int plantId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
