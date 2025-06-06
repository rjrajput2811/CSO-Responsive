using CSO.Core.Models;

namespace CSO.Core.Repositories.NearestPlantRepo;

public interface INearestPlantRepository
{
    Task<List<NearestPlantViewModel>> GetNearestPlantListByPlantIdAsync(int plantId);
}
