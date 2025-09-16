using CSO.Core.Models;

namespace CSO.Core.Repositories.EmailOTPsRepo;

public interface IEmailOTPsRepository
{
    Task<EmailOTPViewModel> GetExistingNotExpiredOTP(string email);
    Task<OperationResult> CreateOTPAsync(EmailOTPViewModel model);
    Task<OperationResult> DeleteOTPAsync(string email);
    Task<OperationResult> CheckEmailAndOTPAsync(string email, int otp, DateTime expirationTime);
    Task<OperationResult> DeleteExpiredOTPAsync(string email);
}
