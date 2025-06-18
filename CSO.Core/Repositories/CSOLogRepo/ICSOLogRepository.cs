using CSO.Core.Models;

namespace CSO.Core.Repositories.CSOLogRepo;

public interface ICSOLogRepository
{
    Task<List<CSOLogViewModel>> GetCSOListAsync();
}
