using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class PlantController : Controller
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ISystemLogService _systemLogService;
        public PlantController(IPlantRepository plantRepository, ISystemLogService systemLogService, IDivisionRepository divisionRepository)
        {
            _plantRepository = plantRepository;
            _divisionRepository = divisionRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult Plant()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllPlant()
        {
            var plantList = await _plantRepository.GetPlantList();
            return Json(plantList);
        }

        [HttpGet]
        public async Task<JsonResult> GetFillPlant()
        {
            var plantList = await _plantRepository.GetDrpPlantList();
            return Json(plantList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var brandbyId = await _plantRepository.GetByIdAsync(Id);
            return Json(brandbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(Plant model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _plantRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _plantRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(Plant model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _plantRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _plantRepository.UpdateAsync(model);
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
                var operationResult = await _plantRepository.DeleteAsync(id);
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
