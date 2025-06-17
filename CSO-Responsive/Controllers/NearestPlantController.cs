using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.NearestPlantRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class NearestPlantController : Controller
    {
        private readonly INearestPlantRepository _nearestPlantRepository;
        private readonly ISystemLogService _systemLogService;
        public NearestPlantController(INearestPlantRepository nearestPlantRepository, ISystemLogService systemLogService)
        {
            _nearestPlantRepository = nearestPlantRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult NearestPlant()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllNearestPlant()
        {
            var plantList = await _nearestPlantRepository.GetNearestPlantList();
            return Json(plantList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var brandbyId = await _nearestPlantRepository.GetByIdAsync(Id);
            return Json(brandbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(NearestPlant model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _nearestPlantRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _nearestPlantRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(NearestPlant model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _nearestPlantRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _nearestPlantRepository.UpdateAsync(model);
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
                var operationResult = await _nearestPlantRepository.DeleteAsync(id);
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
