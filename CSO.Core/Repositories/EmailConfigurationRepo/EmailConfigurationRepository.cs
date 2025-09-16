using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Text;

namespace CSO.Core.Repositories.EmailConfigurationRepo;

public class EmailConfigurationRepository : SqlTableRepository, IEmailConfigurationRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public EmailConfigurationRepository(CSOResponsiveDbContext dbContext,
                                        ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<EmailConfigurationViewModel> GetEmailConfiguration()
    {
        try
        {
            var result = await _dbContext.EmailConfigurations
                .Select(x => new EmailConfigurationViewModel
                {
                    Id = x.Id,
                    From = x.From,
                    SMTPUserName = x.UserName,
                    SMTPPassword = x.Password,
                    SmtpServer = x.SmtpServer,
                    Port = x.Port,
                    SslRequired = x.SslRequired
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CreateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel)
    {
        try
        {
            var newEmailConfig = new EmailConfiguration
            {
                From = emailConfigurationModel.From,
                UserName = emailConfigurationModel.SMTPUserName,
                Password = emailConfigurationModel.SMTPPassword,
                SmtpServer = emailConfigurationModel.SmtpServer,
                Port = emailConfigurationModel.Port,
                SslRequired = emailConfigurationModel.SslRequired,
                AddedBy = emailConfigurationModel.AddedBy,
                AddedOn = emailConfigurationModel.AddedOn

            };

            var result = await base.CreateAsync<EmailConfiguration>(newEmailConfig);
            return result; ;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel)
    {
        try
        {
            var emailConfigToUpdate = await base.GetByIdAsync<EmailConfiguration>(emailConfigurationModel.Id);
            emailConfigToUpdate.From = emailConfigurationModel.From;
            emailConfigToUpdate.UserName = emailConfigurationModel.SMTPUserName;
            emailConfigToUpdate.Password = emailConfigurationModel.SMTPPassword;
            emailConfigToUpdate.SmtpServer = emailConfigurationModel.SmtpServer;
            emailConfigToUpdate.Port = emailConfigurationModel.Port;
            emailConfigToUpdate.SslRequired = emailConfigurationModel.SslRequired;
            emailConfigToUpdate.UpdatedBy = emailConfigurationModel.UpdatedBy;
            emailConfigToUpdate.UpdatedOn = emailConfigurationModel.UpdatedOn;

            var result = await base.UpdateAsync<EmailConfiguration>(emailConfigToUpdate);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> SendForgotPassword(string tempPassword, string userEmail)
    {
        try
        {
            var data = await _dbContext.EmailConfigurations.Where(x => x.Id > 0 && x.SmtpServer != null).FirstOrDefaultAsync();
            if (data != null)
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(data.UserName);
                email.From.Add(email.Sender);

                email.To.Add(MailboxAddress.Parse(userEmail));

                string ssubject = "CSO Login Credentials";
                string sBody = "Hi User ;</BR> Please use Password " + tempPassword + " for login ";

                email.Subject = ssubject.ToString().Trim();
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = sBody.ToString();

                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.CheckCertificateRevocation = false;
                    smtp.Connect(data.SmtpServer, data.Port, SecureSocketOptions.Auto);
                    smtp.Authenticate(data.UserName, data.Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }

    }

    public (string, string) GenerateMailBody(string mailbody, string mailHeader, string mailFooter, string subject, int mailType, string CsoNo, string CsoDT, string Location, string ProdLine, string Description, string ProductCode, string BatchCode, string Qty, string CsoUrl, string status)
    {
        try
        {
            StringBuilder sbMailBody = new StringBuilder();
            var mailbodyDynamic = mailbody;
            StringBuilder sbMailSubject = new StringBuilder();
            var mailSubjectDynamic = subject;
            List<KeyValuePair<EmailTemplateKey, string>> keys =
            [
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSONo, CsoNo),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSODT, CsoDT),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOLocation, Location),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOProdLine, ProdLine),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOProductCode, ProductCode),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSODescription, Description),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOBatchCode, BatchCode),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOQty, Qty),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOURL, CsoUrl),
                new KeyValuePair<EmailTemplateKey, string>(EmailTemplateKey.CSOStatus, status),
            ];


            sbMailBody.Append(mailHeader);
            sbMailBody.AppendLine("<br>");
            sbMailBody.AppendLine("<br>");
            sbMailBody.Append(mailbodyDynamic);
            sbMailBody.AppendLine("<br>");
            sbMailBody.Append(mailFooter);

            foreach (var item in keys)
            {
                sbMailBody.Replace("[[" + item.Key.ToString() + "]]", item.Value);
            }

            sbMailSubject.Append(mailSubjectDynamic);
            foreach (var item in keys)
            {
                sbMailSubject.Replace("[[" + item.Key.ToString() + "]]", item.Value);
            }

            return (sbMailBody.ToString(), sbMailSubject.ToString());
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<bool> SendEmailTrigger(string stakeHolderEmailIds, string userEmails, string createdUser, string mailSubject, string mailBody)
    {
        try
        {
            List<string> ToEmail = new List<string>();
            List<string> CcEmail = new List<string>();

            var data = await _dbContext.EmailConfigurations.Where(x => x.Id > 0 && x.SmtpServer != null).FirstOrDefaultAsync();

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(data.UserName);
            email.From.Add(email.Sender);

            if (!string.IsNullOrEmpty(createdUser))
            {
                email.To.Add(MailboxAddress.Parse(createdUser));
            }

            if (stakeHolderEmailIds != null)
            {
                var emailIds = stakeHolderEmailIds.Split(',');

                for (int m = 0; m <= emailIds.Count() - 1; m++)
                {
                    if (!string.IsNullOrEmpty(emailIds[m]))
                    {
                        email.Cc.Add(MailboxAddress.Parse(emailIds[m]));
                        CcEmail.Add(emailIds[m]);
                    }

                }
            }

            if (userEmails != null)
            {
                var emailIds = userEmails.Split(',');

                for (int m = 0; m <= emailIds.Count() - 1; m++)
                {
                    if (!string.IsNullOrEmpty(emailIds[m]))
                    {
                        email.To.Add(MailboxAddress.Parse(emailIds[m]));
                        ToEmail.Add(emailIds[m]);
                    }

                }
            }

            email.Subject = mailSubject.ToString().Trim();
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailBody.ToString();

            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.CheckCertificateRevocation = false;
                smtp.Connect(data.SmtpServer, data.Port, SecureSocketOptions.Auto);
                smtp.Authenticate(data.UserName, data.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            return true;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            return false;
        }
    }

    public async Task<bool> SendOTPEmailAsync(string userEmail, int otp)
    {
        try
        {
            var data = await _dbContext.EmailConfigurations.Where(x => x.Id > 0 && x.SmtpServer != null).FirstOrDefaultAsync();
            if (data != null)
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(data.UserName)
                };
                email.From.Add(email.Sender);

                email.To.Add(MailboxAddress.Parse(userEmail));

                string ssubject = "Your One-Time Password (OTP)";
                string sBody = $"Your OTP for CSO login is: <b>{otp}</b>";

                email.Subject = ssubject.ToString().Trim();
                BodyBuilder bodyBuilder = new()
                {
                    HtmlBody = sBody.ToString()
                };

                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.CheckCertificateRevocation = false;
                smtp.Connect(data.SmtpServer, data.Port, SecureSocketOptions.Auto);
                smtp.Authenticate(data.UserName, data.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            return false;
        }
    }
}
