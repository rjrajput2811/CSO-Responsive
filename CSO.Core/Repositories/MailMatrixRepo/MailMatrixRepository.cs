using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Mail;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CSO.Core.Repositories.MailMatrixRepo;

public class MailMatrixRepository : SqlTableRepository, IMailMatrixRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    public MailMatrixRepository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SendForgotPassword(string tempPassword, string userEmail)
    {
        try
        {
            var data = await _dbContext.EmailConfigurations.Where(x => x.Id > 0 && x.SmtpServer != null).FirstOrDefaultAsync();
            if (data != null)
            {
                try
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
                catch (Exception ex)
                {
                    throw;

                }
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            throw;
        }

    }
}
