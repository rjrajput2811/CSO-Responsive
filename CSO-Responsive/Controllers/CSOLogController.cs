using CSO.Core.Models;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.CategoryRepo;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSO_Responsive.Controllers
{
    public class CSOLogController : Controller
    {
        private readonly ICSOLogRepository _csoLogRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CSOLogController(ICSOLogRepository csoLogRepository,
                                ISystemLogService systemLogService,
                                IDivisionRepository divisionRepository,
                                ICategoryRepository categoryRepository)
        {
            _csoLogRepository = csoLogRepository;
            _systemLogService = systemLogService;
            _divisionRepository = divisionRepository;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CSOLogAsync(int Id)
        {
            var divisions = await _divisionRepository.GetDivisionList();
            var divisionList = divisions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToList();
            ViewBag.DivisionList = divisionList;

            var categorys = await _categoryRepository.GetCategorysListAsync();
            var categoryList = categorys.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToList();
            ViewBag.CategoryList = categoryList;

            var model = new CSOLogViewModel();
            if (Id > 0)
            {

            }
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetCSOLogListAsync()
        {
            var brandList = await _csoLogRepository.GetCSOLogListAsync();
            return Json(brandList);
        }
    }
}
