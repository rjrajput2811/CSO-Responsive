using CSO.Core.Models;

namespace CSO.Core.Repositories.PlantRepo;

public interface IPlantRepository
{
    Task<List<PlantViewModel>> GetPlantListByDivisionIdAsync(int divisionId);
}
