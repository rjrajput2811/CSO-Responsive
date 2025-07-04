namespace CSO.Core.Repositories.MailMatrixRepo;

public interface IMailMatrixRepository
{
     Task<bool> SendForgotPassword(string tempPassword,string userEmail);
}
