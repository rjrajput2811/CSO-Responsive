using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.EmailConfigurationRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Repositories.Shared;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Security;
using CSO.Core.Services;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CSO.Core.Repositories.MailMatrixRepo;

public class MailMatrixRepository : SqlTableRepository, IMailMatrixRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    private readonly ICSOLogRepository _csoLogRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlantRepository _plantRepository;
    private readonly IDivisionRepository _divisionRepository;
    private readonly IProductTypeRepository _productTypeRepository;
    private readonly IEmailConfigurationRepository _emailConfigurationRepository;

    public MailMatrixRepository(CSOResponsiveDbContext dbContext,
                                ISystemLogService systemLogService,
                                ICSOLogRepository csoLogRepository,
                                IUserRepository userRepository,
                                IPlantRepository plantRepository,
                                IDivisionRepository divisionRepository,
                                IProductTypeRepository productTypeRepository,
                                IEmailConfigurationRepository emailConfigurationRepository) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
        _csoLogRepository = csoLogRepository;
        _userRepository = userRepository;
        _plantRepository = plantRepository;
        _divisionRepository = divisionRepository;
        _productTypeRepository = productTypeRepository;
        _emailConfigurationRepository = emailConfigurationRepository;
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
                RecipientUsers = string.Empty,
                Subject = model.Subject,
                MessageHeader = model.MessageHeader,
                MailBody = model.MailBody,
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
            getRecordToUpdate.RecipientUsers = string.Empty;
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

    public async Task<OperationResult> CSOMailTrigger(int csoId, int mailTypeId, string hostUrl)
    {
        try
        {
            var mailMatrixConfig = await _dbContext.MailMatrices.Where(i => i.MailType == mailTypeId).FirstOrDefaultAsync();
            if (mailMatrixConfig == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Mail Matrix is not configured. Please configure Mail Matrix to send mail."
                };
            }
            var csoLogData = await _csoLogRepository.GetCSOLogById(csoId);

            var user = await _userRepository.GetUserByIdAsync(csoLogData.AddedBy);

            var allUsers = await _dbContext.Users.Where(i => i.IsInMailMatrix == true).ToListAsync();

            var finalUserList = allUsers
                .Where(i => i.DivisionId
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Contains(csoLogData.DivisionId) &&
                    i.PlantId
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Contains(csoLogData.PlantId) &&
                    i.NearestPlantId
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Contains(csoLogData.NearestPlantId) &&
                    (
                        i.ProductTypeId.Contains('[')
                            ? i.ProductTypeId
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Contains($"[{csoLogData.BrandId}-{csoLogData.ProductTypeId}]")
                            : (
                                i.BrandId
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(int.Parse)
                                    .Contains(csoLogData.BrandId) &&
                                i.ProductTypeId
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(int.Parse)
                                    .Contains(csoLogData.ProductTypeId)
                            )
                    )
                )
                .ToList();

            var usersList = new List<string>();

            foreach (var item in finalUserList)
            {
                usersList.Add(item.Email);
            }

            if (!string.IsNullOrEmpty(mailMatrixConfig.RecipientUsers))
            {
                mailMatrixConfig.RecipientUsers = string.Empty;
            }
            mailMatrixConfig.RecipientUsers = string.Join(",", usersList);

            var stakeholderUserList = mailMatrixConfig.StakeHoldersEmailIds;

            var CsoNo = Convert.ToString(100 + csoLogData.Id);
            var CsoDT = csoLogData.Logdate.ToString("dd-MM-yyyy");

            var plant = await _plantRepository.GetByIdAsync(csoLogData.PlantId);
            var location = plant?.Name;

            var description = csoLogData.Description;
            var division = await _divisionRepository.GetByIdAsync(csoLogData.DivisionId);
            var prodLine = division?.Name;

            var productType = await _productTypeRepository.GetByIdAsync(csoLogData.ProductTypeId);
            var prodCode = productType?.Name;

            var batchCode = csoLogData.Batch;
            var qty = Convert.ToString(csoLogData.Quantity);

            var status = from Status d in Enum.GetValues(typeof(Status))
                         select new { ID = (int)d, Name = d.ToString() };

            var csoStatus = status.Where(s => s.ID == csoLogData.Status1).Select(s => s.Name).First();

            var encryptId = CommonService.EncryptStringAES(csoLogData?.Id.ToString());

            var CSOlogURL = $"{hostUrl}/CSOLogAnalysisSolution/CSOLogAnalysisDetails/" + encryptId;

            (string mailBody, string mailSubject) = _emailConfigurationRepository.GenerateMailBody(mailMatrixConfig.MailBody, mailMatrixConfig.MessageHeader, 
                mailMatrixConfig.MessageFooter, mailMatrixConfig.Subject, mailMatrixConfig.MailType, CsoNo, CsoDT, location, prodLine, description, prodCode, batchCode, 
                qty, CSOlogURL, csoStatus);

            var response = await _emailConfigurationRepository.SendEmailTrigger(stakeholderUserList, mailMatrixConfig.RecipientUsers, user.Email, mailSubject, mailBody);

            if (!response)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "An unexpected error occured while sending mail. Please contact administration."
                };
            }

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            return new OperationResult
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}
