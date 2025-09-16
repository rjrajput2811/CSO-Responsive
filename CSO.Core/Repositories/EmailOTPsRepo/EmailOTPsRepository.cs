using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace CSO.Core.Repositories.EmailOTPsRepo;

public class EmailOTPsRepository : SqlTableRepository, IEmailOTPsRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public EmailOTPsRepository(CSOResponsiveDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<EmailOTPViewModel> GetExistingNotExpiredOTP(string email)
    {
        try
        {
            var result = await _dbContext.EmailOTPs
                .Where(i => i.Email == email && i.ExpiresAt > DateTime.Now)
                .Select(x => new EmailOTPViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    OTP = x.OTP,
                    ExpiresAt = x.ExpiresAt
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

    public async Task<OperationResult> CreateOTPAsync(EmailOTPViewModel model)
    {
        try
        {
            var otpRecord = new EmailOTP
            {
                Email = model.Email,
                OTP = model.OTP,
                ExpiresAt = model.ExpiresAt
            };

            var result = await base.CreateAsync<EmailOTP>(otpRecord);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteOTPAsync(string email)
    {
        try 
        {
            var existingRecords = await _dbContext.EmailOTPs
                .Where(i => i.Email == email)
                .ToListAsync();

            foreach (var record in existingRecords)
            {
                var result = await base.DeleteAsync<EmailOTP>(record.Id);
            }

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> CheckEmailAndOTPAsync(string email, int otp, DateTime expirationTime)
    {
        try
        {
            var result = await _dbContext.EmailOTPs
                .AnyAsync(i => i.Email == email && i.OTP == otp && i.ExpiresAt <= expirationTime);

            if (result)
            {
                return new OperationResult { Success = true };
            }
            else {
                return new OperationResult { Success = false, Message = "OTP Expired. Please refresh the page and try again." };
            }
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteExpiredOTPAsync(string email)
    {
        try
        {
            var expiredOTPRecords = await _dbContext.EmailOTPs
                .Where(i => i.Email == email && i.ExpiresAt <= DateTime.Now)
                .ToListAsync();

            foreach (var rec in expiredOTPRecords) 
            {
                var result = await base.DeleteAsync<EmailOTP>(rec.Id);
                if(!result.Success) { return result; }
            }

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
