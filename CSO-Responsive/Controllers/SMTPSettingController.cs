using CSO.Core.Models;
using CSO.Core.Repositories.EmailConfigurationRepo;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers;

public class SMTPSettingController : BaseController
{
    private readonly IEmailConfigurationRepository _emailConfigurationRepository;

    public SMTPSettingController(IEmailConfigurationRepository emailConfigurationRepository)
    {
        _emailConfigurationRepository = emailConfigurationRepository;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var model = new EmailConfigurationViewModel();
        model = await _emailConfigurationRepository.GetEmailConfiguration();
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateEmailConfigurationAsync(EmailConfigurationViewModel model)
    {
        var response = new OperationResult();
        if(model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            response = await _emailConfigurationRepository.UpdateEmailConfiguration(model);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            response = await _emailConfigurationRepository.CreateEmailConfiguration(model);
        }

        return Json(response);
    }
}
