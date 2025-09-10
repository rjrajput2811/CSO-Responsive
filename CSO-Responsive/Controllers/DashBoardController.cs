using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Services.DashboardService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSO_Responsive.Controllers;

public class DashBoardController : BaseController
{
    private readonly IDashboardService _dashboardService;
    private readonly IDivisionRepository _divisionRepository;
    private readonly IPlantRepository _plantRepository;

    public DashBoardController(IDashboardService dashboardService,
                               IDivisionRepository divisionRepository,
                               IPlantRepository plantRepository)
    {
        _dashboardService = dashboardService;
        _divisionRepository = divisionRepository;
        _plantRepository = plantRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> GetDashboardDataAsync(int fYear, string divisionIds, string plantIds, DateTime? fromDate, DateTime? toDate)
    {
        var userid = HttpContext.Session.GetInt32("UserId") ?? 0;
        var response = await _dashboardService.GetDashboardDataAsync(fYear, userid, divisionIds, plantIds, fromDate, toDate);
        return Json(response);
    }

    public async Task<ActionResult> GetDivisionListAsync()
    {
        var userid = HttpContext.Session.GetInt32("UserId") ?? 0;
        var divisionList = new List<DivisionViewModel>();
        var list = new List<SelectListItem>();
        if(userid > 1)
        {
            divisionList = await _divisionRepository.GetDivisionListByUserAsync(userid);
            list = divisionList.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToList();
        }
        else
        {
            divisionList = await _divisionRepository.GetAllDivisionList();
            list = divisionList.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToList();
        }

        return Json(list);
    }

    public async Task<ActionResult> GetPlantListAsync(string divisionIds)
    {
        var userid = HttpContext.Session.GetInt32("UserId") ?? 0;
        var plantList = new List<PlantViewModel>();
        var list = new List<SelectListItem>();
        if (string.IsNullOrWhiteSpace(divisionIds))
            return Json(new { plantList = list });

        var divisionIdList = divisionIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        if (divisionIdList.Count == 0)
            return Json(new { plantList = list });
        if (userid > 1)
        {
            foreach (var divisionId in divisionIdList)
            {
                plantList = await _plantRepository.GetPlantListByDivisionAndUserAsync(divisionId, userid);
                list.AddRange(plantList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }));
            }
        }
        else
        {
            foreach (var divisionId in divisionIdList)
            {
                plantList = await _plantRepository.GetPlantListByDivisionIdAsync(divisionId);
                list.AddRange(plantList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }));
            }
        }

        return Json(new { plantList = list });
    }
}
