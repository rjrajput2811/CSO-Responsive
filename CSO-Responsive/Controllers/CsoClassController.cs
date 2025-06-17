using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.ComplaintTypeRepo;
using CSO.Core.Repositories.CSOClassRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class CsoClassController : Controller
    {
        private readonly ICSOClassRepository _csoClassRepository;
        private readonly ISystemLogService _systemLogService;

        public CsoClassController(ICSOClassRepository csoClassRepository, ISystemLogService systemLogService)
        {
            _csoClassRepository = csoClassRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult CSOClass()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllCsoClass()
        {
            var csoClassList = await _csoClassRepository.GetCsoClassList();
            return Json(csoClassList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var csoClassbyId = await _csoClassRepository.GetByIdAsync(Id);
            return Json(csoClassbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(CSOClass model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _csoClassRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _csoClassRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(CSOClass model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _csoClassRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _csoClassRepository.UpdateAsync(model);
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
                var operationResult = await _csoClassRepository.DeleteAsync(id);
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
