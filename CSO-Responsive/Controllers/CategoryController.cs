using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.CategoryRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISystemLogService _systemLogService;

    public CategoryController(ICategoryRepository categoryRepository, ISystemLogService systemLogService)
    {
        _categoryRepository = categoryRepository;
        _systemLogService = systemLogService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> GetAllCategory()
    {
        var list = await _categoryRepository.GetCategorysListAsync();
        return Json(list);
    }

    public async Task<JsonResult> GetById(int Id)
    {
        var divbyId = await _categoryRepository.GetByIdAsync(Id);
        return Json(divbyId);
    }

    [HttpPost]
    public async Task<JsonResult> CreateAsync(Categorys model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var operationResult = new OperationResult();
                bool existingResult = await _categoryRepository.CheckDuplicate(model.Name.Trim(), 0);
                if (!existingResult)
                {
                    model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
                    model.AddedOn = DateTime.Now;
                    operationResult = await _categoryRepository.CreateAsync(model);
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
    public async Task<JsonResult> UpdateAsync(Categorys model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var operationResult = new OperationResult();
                bool existingResult = await _categoryRepository.CheckDuplicate(model.Name.Trim(), model.Id);
                if (!existingResult)
                {
                    model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    model.UpdatedOn = DateTime.Now;
                    operationResult = await _categoryRepository.UpdateAsync(model);
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
            var operationResult = await _categoryRepository.DeleteAsync(id);
            return Json(operationResult);
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }

    }
}
