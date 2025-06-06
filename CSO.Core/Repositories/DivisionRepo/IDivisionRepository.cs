using CSO.Core.Models;

namespace CSO.Core.Repositories.DivisionRepo;

public interface IDivisionRepository
{
    Task<List<DivisionViewModel>> GetDivisionList();
}
