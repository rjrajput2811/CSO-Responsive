using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace CSO_Responsive.Controllers
{
    public class DivisionController : Controller
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly ISystemLogService _systemLogService;

        public DivisionController(IDivisionRepository divisionRepository, ISystemLogService systemLogService)
        {
            _divisionRepository = divisionRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult Division()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllDivision()
        {
            var divisionList = await _divisionRepository.GetDivisionList();
            return Json(divisionList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var divbyId = await _divisionRepository.GetByIdAsync(Id);
            return Json(divbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(Division model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _divisionRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        operationResult = await _divisionRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(Division model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _divisionRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        operationResult = await _divisionRepository.UpdateAsync(model);
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
                var operationResult = await _divisionRepository.DeleteAsync(id);
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
