using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.RecycleDayRepo;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSO_Responsive.Controllers
{
    public class RecycleDayController : BaseController
    {
        private readonly IRecycleDayRepository _recycleDayRepository;
        private readonly ISystemLogService _systemLogService;

        public RecycleDayController(IRecycleDayRepository recycleDayRepository, ISystemLogService systemLogService)
        {
            _recycleDayRepository = recycleDayRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllRecycleDayAsync()
        {
            var recycleDayList = await _recycleDayRepository.GetRecycleDayList();
            return Json(recycleDayList);
        }

        [HttpGet]
        public async Task<JsonResult> GetByIdAsync(int Id)
        {
            var divbyId = await _recycleDayRepository.GetByIdAsync(Id);
            return Json(divbyId);
        }

        [HttpPost]
        public async Task<JsonResult> InsertUpdateRecycleDaysAsync(RecycleDayViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var fy = HttpContext.Session.GetString("FYear");
            var fromYear = int.Parse($"20{fy.Substring(0, 2)}");
            var toYear = int.Parse($"20{fy.Substring(2, 2)}");
            var financialYear = int.Parse(fy);

            var fyStart = new DateTime(fromYear, 4, 1);  // 01/04/YYYY
            var fyEnd = new DateTime(toYear, 3, 31); // 31/03/YYYY+1

            // **Validation 1: Check if FromDate & ToDate exist in the same Financial Year**
            if (model.FromDate < fyStart || model.ToDate > fyEnd)
            {
                return Json(new { Success = false, Errors = new List<string> { "From Date and To Date must be within the same financial year." }});
            }

            // **Validation 2: Check if the date range overlaps with an existing entry**
            var isOverlapping = await _recycleDayRepository.IsDateRangeOverlapping(model.Id, model.CSOLogPhase, financialYear, model.FromDate, model.ToDate);
            if (isOverlapping)
            {
                return Json(new { Success = false, Errors = new List<string> { "The selected date range overlaps with an existing entry." }});
            }

            // **Validation 3: Check if any date in range is already used in LogCSO database**
            //var isDateUsed = await _recycleDayRepository.IsDateUsedInLogCso(model.FromDate, model.ToDate, model.CSOLogPhase);
            //if (isDateUsed)
            //{
            //    return Json(new { Success = false, Errors = new List<string> { "One or more selected dates are already used in Log CSO and cannot be changed." }});
            //}

            // If validation passes, proceed to save or update
            if (model.Id > 0)
            {
                model.ModifiedBy = userId;
                model.ModifiedDate = DateTime.Now;
                var result = await _recycleDayRepository.UpdateAsync(model);
                return Json(new { Success = true });
            }
            else
            {
                model.FinancialYear = financialYear;
                model.AddedBy = userId ?? 0;
                model.AddedDate = DateTime.Now;
                var result = await _recycleDayRepository.CreateAsync(model);
                return Json(new { Success = true });
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
