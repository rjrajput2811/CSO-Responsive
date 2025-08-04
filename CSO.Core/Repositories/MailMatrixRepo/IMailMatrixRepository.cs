using CSO.Core.Models;

namespace CSO.Core.Repositories.MailMatrixRepo;

public interface IMailMatrixRepository
{
    Task<MailMatrixViewModel?> GetMailMatrixDetailsAsync(int mailTypeId);
    Task<OperationResult> CreateMailMatrixDetailsAsync(MailMatrixViewModel model, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateMailMatrixDetailsAsync(MailMatrixViewModel model);
    Task<OperationResult> CSOMailTrigger(int csoId, int mailTypeId, string hostUrl);
}
