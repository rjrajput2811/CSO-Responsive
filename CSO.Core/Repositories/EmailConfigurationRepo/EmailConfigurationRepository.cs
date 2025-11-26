using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net;
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

    //public async Task<bool> SendOTPEmailAsync(string userEmail, string otp)
    //{
    //    try
    //    {
    //        var data = await _dbContext.EmailConfigurations.Where(x => x.Id > 0 && x.SmtpServer != null).FirstOrDefaultAsync();
    //        if (data != null)
    //        {
    //            var email = new MimeMessage
    //            {
    //                Sender = MailboxAddress.Parse(data.UserName)
    //            };
    //            email.From.Add(email.Sender);

    //            email.To.Add(MailboxAddress.Parse(userEmail));

    //            string ssubject = "Your One-Time Password (OTP)";
    //            string sBody = $"Your OTP for CSO login is: <b>{otp}</b>";

    //            email.Subject = ssubject.ToString().Trim();
    //            BodyBuilder bodyBuilder = new()
    //            {
    //                HtmlBody = sBody.ToString()
    //            };

    //            email.Body = bodyBuilder.ToMessageBody();

    //            using var smtp = new SmtpClient();
    //            smtp.CheckCertificateRevocation = false;
    //            smtp.Connect(data.SmtpServer, data.Port, SecureSocketOptions.Auto);
    //            smtp.Authenticate(data.UserName, data.Password);
    //            smtp.Send(email);
    //            smtp.Disconnect(true);
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _systemLogService.WriteLog(ex.Message);
    //        return false;
    //    }
    //}

    private bool IsEmail(string input)
    {
        return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public async Task<bool> SendOTPEmailAsync(string userEmail, string otp, string body)
    {
        try
        {
            _systemLogService.WriteLog("SendOTPEmailAsync: Started.");

            var data = await _dbContext.EmailConfigurations
                .Where(x => x.Id > 0 && x.SmtpServer != null)
                .FirstOrDefaultAsync();

            if (data == null)
            {
                _systemLogService.WriteLog("SendOTPEmailAsync: FAILED → No SMTP configuration found.");
                return false;
            }

            _systemLogService.WriteLog(
                $"SendOTPEmailAsync: Loaded SMTP config → Server:{data.SmtpServer}, Port:{data.Port}, SSL:{data.SslRequired}"
            );

            // 🔑 Resolve actual email (support email OR Id/Code like PPS)
            string emailAddress;

            if (IsEmail(userEmail))
            {
                emailAddress = userEmail;
            }
            else
            {
                emailAddress = await _dbContext.Users
                    .Where(u =>
                        u.Id.ToString() == userEmail      // numeric id like PPS
                        || u.UserName == userEmail        // login id (if any)
                        || u.Email == userEmail)   // employee code (if you have this)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                _systemLogService.WriteLog(
                    $"SendOTPEmailAsync: FAILED → No valid email found for '{userEmail}'."
                );
                return false;
            }

            _systemLogService.WriteLog($"SendOTPEmailAsync: Resolved recipient → {emailAddress}");

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(data.UserName);
            email.From.Add(email.Sender);
            email.To.Add(MailboxAddress.Parse(emailAddress));

            string ssubject = "Your One-Time Password (OTP)";
            //string sBody = "Your OTP for CSO login is: <b>{otp}</b>";

            email.Subject = (ssubject ?? "No Subject").Trim();


            //var bodyBuilder = new BodyBuilder
            //{
            //    HtmlBody = sBody
            //};
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            email.Body = bodyBuilder.ToMessageBody();

            //_systemLogService.WriteLog($"SendOTPEmailAsync: Mail body → {sBody}");

            _systemLogService.WriteLog("SendOTPEmailAsync: Email object created successfully.");

            using (var smtp = new SmtpClient())
            {
                smtp.CheckCertificateRevocation = false;

                // Use the SAME style as your working PPS mail (StartTls)
                _systemLogService.WriteLog(
                    $"SendOTPEmailAsync: Connecting to SMTP → {data.SmtpServer}:{data.Port} (StartTls)"
                );

                smtp.Connect(data.SmtpServer, data.Port, SecureSocketOptions.StartTls);
                _systemLogService.WriteLog("SendOTPEmailAsync: SMTP connection successful.");

                smtp.Authenticate(data.UserName, data.Password);
                _systemLogService.WriteLog("SendOTPEmailAsync: SMTP authentication successful.");

                smtp.Send(email);
                _systemLogService.WriteLog("SendOTPEmailAsync: Email handed to SMTP server.");

                smtp.Disconnect(true);
            }

            _systemLogService.WriteLog(
                $"SendOTPEmailAsync: SUCCESS → OTP email sent to {emailAddress}."
            );

            return true;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog($"SendOTPEmailAsync ERROR → {ex}");
            return false;
        }
    }

    public Task<string> GenerateOtpLoginEmailBody(string otp)
    {
        string body = $@"
    <div style='font-family:Arial;font-size:14px;color:#333;'>
        <p>Your OTP for login is:</p>
        <h2>{WebUtility.HtmlEncode(otp)}</h2>
        <p>This OTP is valid for the next 10 minutes.</p>
        <p>Regards,<br>CSO Team</p>
    </div>";

        return Task.FromResult(body);
    }

}
