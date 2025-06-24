using CSO.Core.Models;

namespace CSO.Core.Repositories.CSOLogFileRepo;

public interface ICSOLogFileRepository
{
    Task<OperationResult> InsertCSOLogFileInfoAsync(CSOLogFileViewModel model);
}
