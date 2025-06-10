using CSO.Core.Models;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.NearestPlantRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Repositories.UsersRoleRepo;
using CSO.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSO_Responsive.Controllers;

public class UsersController : Controller
{
    private readonly IUsersRoleRepository _usersRoleRepository;
    private readonly IDivisionRepository _divisionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlantRepository _plantRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly INearestPlantRepository _nearestPlantRepository;
    private readonly IProductTypeRepository _productTypeRepository;

    public UsersController(IUsersRoleRepository usersRoleRepository,
                           IDivisionRepository divisionRepository,
                           IUserRepository userRepository,
                           IPlantRepository plantRepository,
                           IBrandRepository brandRepository,
                           INearestPlantRepository nearestPlantRepository,
                           IProductTypeRepository productTypeRepository)
    {
        _usersRoleRepository = usersRoleRepository;
        _divisionRepository = divisionRepository;
        _userRepository = userRepository;
        _plantRepository = plantRepository;
        _brandRepository = brandRepository;
        _nearestPlantRepository = nearestPlantRepository;
        _productTypeRepository = productTypeRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> UserDetailsAsync(int Id)
    {
        var model = new UserViewModel();

        var userRoles = await _usersRoleRepository.GetUserRolesAsync();
        var roleList = userRoles.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.RoleName
        })
        .ToList();
        ViewBag.RoleList = roleList;

        var userTypeList = Enum.GetValues(typeof(UserType))
            .Cast<UserType>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = x.ToString()
            })
            .ToList();
        ViewBag.UserTypeList = userTypeList;
        model.RoleId = -1;
        model.UserType = -1;

        var divisions = await _divisionRepository.GetDivisionList();
        var divisionList = divisions.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.DivisionList = divisionList;

        if (Id > 0)
        {
            model = await _userRepository.GetUserByIdAsync(Id);
        }
        
        return View(model);
    }

    public async Task<ActionResult> GetDivisionListAsync()
    {
        var divisionResult = await _divisionRepository.GetDivisionList();
        var divisionList = divisionResult.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();

        return Json(divisionList);
    }

    public async Task<ActionResult> GetPlantListAndBrandListByDivisionAsync(string divisionIds)
    {
        var plantList = new List<SelectListItem>();
        var brandList = new List<SelectListItem>();
        if (string.IsNullOrWhiteSpace(divisionIds))
            return Json(new { plantList, brandList });

        var divisionIdList = divisionIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        if (divisionIdList.Count == 0)
            return Json(new { plantList, brandList });

        foreach (var divisionId in divisionIdList)
        {
            var plantResult = await _plantRepository.GetPlantListByDivisionIdAsync(divisionId);
            plantList.AddRange(plantResult.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }));

            var brandResult = await _brandRepository.GetBrandListByDivisionIdAsync(divisionId);
            brandList.AddRange(brandResult.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }));
        }

        plantList = plantList.GroupBy(g => g.Value).Select(x => x.First()).OrderBy(o => o.Text).ToList();
        brandList = brandList.GroupBy(g => g.Value).Select(x => x.First()).OrderBy(o => o.Text).ToList();
        
        return Json(new { plantList, brandList });
    }

    public async Task<ActionResult> GetNearestPlantByPlantAsync(string plantIds)
    {
        var nearestPlantList = new List<SelectListItem>();
        if (string.IsNullOrWhiteSpace(plantIds))
            return Json(nearestPlantList);

        var plantIdList = plantIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        if (plantIdList.Count == 0)
            return Json(nearestPlantList);

        foreach(var plantId in plantIdList)
        {
            var result = await _nearestPlantRepository.GetNearestPlantListByPlantIdAsync(plantId);
            nearestPlantList.AddRange(result.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }));
        }

        nearestPlantList = nearestPlantList.GroupBy(g => g.Value).Select(x => x.First()).OrderBy(o => o.Text).ToList();

        return Json(nearestPlantList);
    }

    public async Task<ActionResult> GetProductTypeByBrandAsync(string brandIds)
    {
        var productTypeList = new List<SelectListItem>();
        if (string.IsNullOrWhiteSpace(brandIds))
            return Json(productTypeList);

        var brandIdList = brandIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        if (brandIdList.Count == 0)
            return Json(productTypeList);

        foreach(var brandId in brandIdList)
        {
            var result = await _productTypeRepository.GetProductTypeListByBrandIdAsync(brandId);
            productTypeList.AddRange(result.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }));
        }

        productTypeList = productTypeList.GroupBy(g => g.Value).Select(x => x.First()).OrderBy(o => o.Text).ToList();

        return Json(productTypeList);
    }

    public async Task<ActionResult> GetUsersListAsync()
    {
        var usersList = await _userRepository.GetAllUsersAsync();
        return Json(usersList);
    }

    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        var result = await _userRepository.DeleteUserAsync(id);
        return Json(result);
    }

    public async Task<ActionResult> InsertUpdateUserAsync(UserViewModel userViewModel)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if (userViewModel.Id > 0)
        {
            userViewModel.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            userViewModel.UpdatedOn = DateTime.Now;
            var updateResult = await _userRepository.UpdateUserAsync(userViewModel);
            return Json(updateResult);
        }
        else
        {
            userViewModel.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            userViewModel.AddedOn = DateTime.Now;
            var insertResult = await _userRepository.InsertUserAsync(userViewModel);
            return Json(insertResult);
        }
    }
}
