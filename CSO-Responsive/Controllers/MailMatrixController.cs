using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.MailMatrixRepo;
using CSO.Core.Repositories.UserRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSO_Responsive.Controllers;

public class MailMatrixController : BaseController
{
    private readonly IMailMatrixRepository _mailMatrixRepository;
    private readonly IUserRepository _userRepository;

    public MailMatrixController(IMailMatrixRepository mailMatrixRepository,
                                IUserRepository userRepository)
    {
        _mailMatrixRepository = mailMatrixRepository;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> GetStakeHolderList()
    {
        var result = await _userRepository.GetAllUsersAsync();
        var list = result.Select(x => new SelectListItem
        {
            Value = x.Email,
            Text = x.Email
        })
        .ToList();

        return Json(list);
    }

    public async Task<ActionResult> GetMailMatrixDetailsbyMailTypeAsync(int mailTypeId)
    {
        var response = await _mailMatrixRepository.GetMailMatrixDetailsAsync(mailTypeId);
        return Json(response);
    }

    public async Task<ActionResult> InsertUpdateMailMatrixDetailsAsync(MailMatrixViewModel model)
    {
        if(model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.UpdatedOn = DateTime.Now;
            var response = await _mailMatrixRepository.UpdateMailMatrixDetailsAsync(model);
            return Json(response);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var response = await _mailMatrixRepository.CreateMailMatrixDetailsAsync(model, true);
            var createdData = response.Payload as MailMatrix;
            response.ObjectId = createdData?.Id;
            return Json(response);
        }
    }
}
