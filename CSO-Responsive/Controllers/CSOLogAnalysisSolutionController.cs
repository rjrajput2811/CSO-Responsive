using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.CategoryRepo;
using CSO.Core.Repositories.CSOLogAnalysisRepo;
using CSO.Core.Repositories.CSOLogFileRepo;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.NearestPlantRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class CSOLogAnalysisSolutionController : Controller
    {
        private readonly ICSOLogAnalysisRepository _csoLogAnalRepository;
        private readonly ISystemLogService _systemLogService;

        public CSOLogAnalysisSolutionController(ICSOLogAnalysisRepository csoLogAnalRepository,
                                ISystemLogService systemLogService)
        {
            _csoLogAnalRepository = csoLogAnalRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult CSOLogAnalysisSolution()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCSOLogAnalListAsync()
        {
            string fYear = HttpContext.Session.GetString("FYear") ?? "";
            var csoList = await _csoLogAnalRepository.GetCSOLogListAsync(fYear);
            return Json(csoList);
        }

        public IActionResult CSOLogAnalysisDetails()
        {
            return View();
        }
    }
}
