using CSO.Core.Models;

namespace CSO.Core.Repositories.EmailConfigurationRepo;

public interface IEmailConfigurationRepository
{
    Task<EmailConfigurationViewModel> GetEmailConfiguration();
    Task<OperationResult> CreateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel);
    Task<OperationResult> UpdateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel);
    Task<bool> SendForgotPassword(string tempPassword, string userEmail);
    (string, string) GenerateMailBody(string mailbody, string mailHeader, string mailFooter, string subject, int mailType, string CsoNo, string CsoDT, string Location, string ProdLine, string Description, string ProductCode, string BatchCode, string Qty, string CsoUrl, string status);
    Task<bool> SendEmailTrigger(string stakeHolderEmailIds, string userEmails, string createdUser, string mailSubject, string mailBody);
}
