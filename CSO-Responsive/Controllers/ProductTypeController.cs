using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ISystemLogService _systemLogService;
        public ProductTypeController(IProductTypeRepository productTypeRepository, ISystemLogService systemLogService)
        {
            _productTypeRepository = productTypeRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult ProductType()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllProdType()
        {
            var brandList = await _productTypeRepository.GetProdTypeList();
            return Json(brandList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var brandbyId = await _productTypeRepository.GetByIdAsync(Id);
            return Json(brandbyId);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(ProductType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _productTypeRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.AddedOn = DateTime.Now;
                        model.AddedBy = 1;
                        operationResult = await _productTypeRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync(ProductType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _productTypeRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedOn = DateTime.Now;
                        model.UpdatedBy = 1;
                        operationResult = await _productTypeRepository.UpdateAsync(model);
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
                var operationResult = await _productTypeRepository.DeleteAsync(id);
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
