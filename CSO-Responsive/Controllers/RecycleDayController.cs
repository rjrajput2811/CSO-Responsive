using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.RecycleDayRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class RecycleDayController : Controller
    {
        private readonly IRecycleDayRepository _recycleDayRepository;
        private readonly ISystemLogService _systemLogService;

        public RecycleDayController(IRecycleDayRepository recycleDayRepository, ISystemLogService systemLogService)
        {
            _recycleDayRepository = recycleDayRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult RecycleDayConfiguration()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllRecycleDay()
        {
            var recycleDayList = await _recycleDayRepository.GetRecycleDayList();
            return Json(recycleDayList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var divbyId = await _recycleDayRepository.GetByIdAsync(Id);
            return Json(divbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(RecycleDay model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var financialYear = 0;
                    var operationResult = new OperationResult();
                    bool existingResult = false;
                    if (!existingResult)
                    {
                        var fy = HttpContext.Session.GetString("FYear");
                        if (fy != null)
                        {
                            var fromYear = int.Parse($"20{fy.Substring(0, 2)}");
                            var toYear = int.Parse($"20{fy.Substring(2, 2)}");
                            financialYear = int.Parse(fy);
                        }

                        model.FinancialYear = financialYear;
                        model.AddedDate = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _recycleDayRepository.CreateAsync(model);
                        return Json(operationResult);
                    }
                    else
                    {
                        operationResult.Message = "Exist";
                        return Json(operationResult);
                    }
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync(RecycleDay model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = false;
                    if (!existingResult)
                    {
                        model.ModifiedDate = DateTime.Now;
                        model.ModifiedBy = 1;
                        operationResult = await _recycleDayRepository.UpdateAsync(model);
                        return Json(operationResult);
                    }
                    else
                    {
                        operationResult.Message = "Exist";
                        return Json(operationResult);
                    }
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAsync(int id)
        {
            try
            {
                var operationResult = await _recycleDayRepository.DeleteAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }
    }
}
