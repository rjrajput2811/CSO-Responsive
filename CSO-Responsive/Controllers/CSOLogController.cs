using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.CategoryRepo;
using CSO.Core.Repositories.CSOLogFileRepo;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.NearestPlantRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using CSO_Responsive.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

namespace CSO_Responsive.Controllers;

public class CSOLogController : Controller
{
    private readonly ICSOLogRepository _csoLogRepository;
    private readonly ISystemLogService _systemLogService;
    private readonly IDivisionRepository _divisionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IPlantRepository _plantRepository;
    private readonly INearestPlantRepository _nearestPlantRepository;
    private readonly IProductTypeRepository _productTypeRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICSOLogFileRepository _csoLogFileRepository;

    public CSOLogController(ICSOLogRepository csoLogRepository,
                            ISystemLogService systemLogService,
                            IDivisionRepository divisionRepository,
                            ICategoryRepository categoryRepository,
                            IBrandRepository brandRepository,
                            IPlantRepository plantRepository,
                            INearestPlantRepository nearestPlantRepository,
                            IProductTypeRepository productTypeRepository,
                            IUserRepository userRepository,
                            ICSOLogFileRepository csoLogFileRepository)
    {
        _csoLogRepository = csoLogRepository;
        _systemLogService = systemLogService;
        _divisionRepository = divisionRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _plantRepository = plantRepository;
        _nearestPlantRepository = nearestPlantRepository;
        _productTypeRepository = productTypeRepository;
        _userRepository = userRepository;
        _csoLogFileRepository = csoLogFileRepository;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetCSOLogListAsync()
    {
        string fYear = HttpContext.Session.GetString("FYear") ?? "";
        var csoList = await _csoLogRepository.GetCSOLogListAsync(fYear);
        return Json(csoList);
    }

    public async Task<IActionResult> CSOLogAsync(string id)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var divisions = new List<DivisionViewModel>();
        var brands = new List<BrandViewModel>();
        var plants = new List<PlantViewModel>();
        var nearstPlants = new List<NearestPlantViewModel>();
        var productTypes = new List<ProductTypeViewModel>();

        if (userId == 1)
        {
            divisions = await _divisionRepository.GetDivisionList();
        }
        else
        {
            divisions = await _divisionRepository.GetDivisionListByUserAsync(userId);
        }

        var divisionList = divisions.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.DivisionList = divisionList;

        var categorys = await _categoryRepository.GetCategorysListAsync();
        var categoryList = categorys.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.CategoryList = categoryList;

        var model = new CSOLogViewModel();
        if (!string.IsNullOrEmpty(id))
        {
            string base64 = id.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
                case 1: base64 += "="; break;
            }
            byte[] encryptedBytes = Convert.FromBase64String(base64);

            string decryptedText = DecryptFromAes(encryptedBytes);
            if (!int.TryParse(decryptedText, out int Id))
                return BadRequest("Invalid decrypted ID");
            model = await _csoLogRepository.GetCSOLogById(Id);
        }
        else
        {
            model.UserName = _userRepository?.GetUserByIdAsync(userId).Result?.Name;
            model.Logdate = DateTime.Now;
        }
            return View(model);
    }

    public string DecryptFromAes(byte[] cipherBytes)
    {
        var key = Encoding.UTF8.GetBytes("8080808080808080");
        var iv = Encoding.UTF8.GetBytes("8080808080808080");

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(cipherBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd(); // This will be the original ID as string
    }

    

    public async Task<ActionResult> GetBrandListAndPlantListByDivisinAndUserAsync(int divisionId)
    {
        var brandResult = await _brandRepository.GetBrandListByDivisionIdAsync(divisionId);
        var plantResult = await _plantRepository.GetPlantListByDivisionIdAsync(divisionId);

        var brandList = brandResult.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        var plantList = plantResult.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();

        return Json(new  { brandList, plantList });
    }

    public async Task<ActionResult> GetProductTypeListByBrandAndUserAsync(int brandId)
    {
        var productTypeList = await _productTypeRepository.GetProductTypeListByBrandIdAsync(brandId);

        var list = productTypeList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        return Json(list);
    }

    public async Task<ActionResult> GetNearestPlantListByPlantAndUserAsync(int plantId)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var nearestPlantList = await _nearestPlantRepository.GetNearestPlantListByPlantIdAsync(plantId);

        var list = nearestPlantList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        return Json(list);
    }

    public async Task<ActionResult> InsertUpdateCSOLogAsync(CSOLogViewModel model)
    {
        var result = new OperationResult();
        if(model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            result = await _csoLogRepository.UpdateCSOLogAsync(model);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            result = await _csoLogRepository.CreateCSOLogAsync(model);
        }

        if (!result.Success) { return Json(result); }

        var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadFolder = "CSOLogandAnalysisSolutionFiles";
        var uploadPath = Path.Combine(wwwRootPath, uploadFolder);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        foreach (var file in model.files)
        {
            var originalFileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var dateStamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

            var uniqueFileName = $"{originalFileNameWithoutExt}_{dateStamp}{extension}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var csoLogFilesModel = new CSOLogFileViewModel
            {
                FilePath = filePath,
                FileName = uniqueFileName,
                Type = (int)CSOLogFileType.CSOLOg,
                CSOLogId = result.ObjectId ?? 0,
                AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0,
                AddedOn = DateTime.Now
            };

            result = await _csoLogFileRepository.InsertCSOLogFileInfoAsync(csoLogFilesModel);
            if (!result.Success) { return Json(result); }
        }

        return Json(result);
    }
}