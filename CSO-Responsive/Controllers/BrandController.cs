using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ISystemLogService _systemLogService;
        public BrandController(IBrandRepository brandRepository, ISystemLogService systemLogService,IDivisionRepository divisionRepository)
        {
            _brandRepository = brandRepository;
            _divisionRepository = divisionRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult Brand()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllBrand()
        {
            var brandList = await _brandRepository.GetBrandList();
            return Json(brandList);
        }

        [HttpGet]
        public async Task<JsonResult> GetFillBrand()
        {
            var brandList = await _brandRepository.GetDrpBrandList();
            return Json(brandList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var brandbyId = await _brandRepository.GetByIdAsync(Id);
            return Json(brandbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(Brand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _brandRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _brandRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(Brand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _brandRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _brandRepository.UpdateAsync(model);
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
                var operationResult = await _brandRepository.DeleteAsync(id);
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
