using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.ComplaintTypeRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class ComplaintTypeController : Controller
    {
        private readonly IComplaintTypeRepository _compTypeRepository;
        private readonly ISystemLogService _systemLogService;

        public ComplaintTypeController(IComplaintTypeRepository compTypeRepository, ISystemLogService systemLogService)
        {
            _compTypeRepository = compTypeRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult ComplaintType()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllComplaintType()
        {
            var compTypeList = await _compTypeRepository.GetComTypeList();
            return Json(compTypeList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var compTypebyId = await _compTypeRepository.GetByIdAsync(Id);
            return Json(compTypebyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(ComplaintType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _compTypeRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _compTypeRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(ComplaintType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _compTypeRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _compTypeRepository.UpdateAsync(model);
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
                var operationResult = await _compTypeRepository.DeleteAsync(id);
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
