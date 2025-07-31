using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CSO.Core.Repositories.MailMatrixRepo;

public class MailMatrixRepository : SqlTableRepository, IMailMatrixRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public MailMatrixRepository(CSOResponsiveDbContext dbContext,
                                ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
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

    public async Task<MailMatrixViewModel?> GetMailMatrixDetailsAsync(int mailTypeId)
    {
        try
        {
            var result = await _dbContext.MailMatrices
                .Where(i => i.MailType == mailTypeId)
                .Select(x => new MailMatrixViewModel
                {
                    Id = x.Id,
                    MailType = x.MailType,
                    StakeHoldersEmailIds = x.StakeHoldersEmailIds,
                    Subject = x.Subject,
                    MessageHeader = x.MessageHeader,
                    MailBody = x.MailBody,
                    MessageFooter = x.MessageFooter
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

    public async Task<OperationResult> CreateMailMatrixDetailsAsync(MailMatrixViewModel model, bool returnCreatedRecord = false)
    {
        try
        {
            var newRecord = new MailMatrix
            {
                MailType = model.MailType,
                StakeHoldersEmailIds = model.StakeHoldersEmailIds,
                Subject = model.Subject,
                MessageHeader = model.MessageHeader,
                MailBody= model.MailBody,
                MessageFooter = model.MessageFooter,
                AddedBy = model.AddedBy,
                AddedOn = model.AddedOn,
                Active = true
            };

            var result = await base.CreateAsync<MailMatrix>(newRecord, returnCreatedRecord);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateMailMatrixDetailsAsync(MailMatrixViewModel model)
    {
        try
        {
            var getRecordToUpdate = await base.GetByIdAsync<MailMatrix>(model.Id);
            getRecordToUpdate.MailType = model.MailType;
            getRecordToUpdate.StakeHoldersEmailIds = model.StakeHoldersEmailIds;
            getRecordToUpdate.Subject = model.Subject;
            getRecordToUpdate.MessageHeader = model.MessageHeader;
            getRecordToUpdate.MailBody = model.MailBody;
            getRecordToUpdate.MessageFooter = model.MessageFooter;
            getRecordToUpdate.UpdatedBy = model.UpdatedBy;
            getRecordToUpdate.UpdatedOn = model.UpdatedOn;

            var result = await base.UpdateAsync<MailMatrix>(getRecordToUpdate);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
