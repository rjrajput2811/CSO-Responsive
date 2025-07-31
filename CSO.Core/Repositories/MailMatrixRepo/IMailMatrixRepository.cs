using CSO.Core.Models;

namespace CSO.Core.Repositories.MailMatrixRepo;

public interface IMailMatrixRepository
{
     Task<bool> SendForgotPassword(string tempPassword,string userEmail);
    Task<MailMatrixViewModel?> GetMailMatrixDetailsAsync(int mailTypeId);
    Task<OperationResult> CreateMailMatrixDetailsAsync(MailMatrixViewModel model, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateMailMatrixDetailsAsync(MailMatrixViewModel model);
}
