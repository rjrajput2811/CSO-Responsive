using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class CSOLogController : Controller
    {
        private readonly ICSOLogRepository _csoLogRepository;
        private readonly ISystemLogService _systemLogService;
        public CSOLogController(ICSOLogRepository csoLogRepository, ISystemLogService systemLogService)
        {
            _csoLogRepository = csoLogRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CSOLog()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCsoList()
        {
            var brandList = await _csoLogRepository.GetCSOListAsync();
            return Json(brandList);
        }
    }
}
