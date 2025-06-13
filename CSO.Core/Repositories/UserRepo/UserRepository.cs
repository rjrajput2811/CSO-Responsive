using CSO.Core.DatabaseContext;
using CSO.Core.Models;
using CSO.Core.Repositories.Shared;
using CSO.Core.Security;
using CSO.Core.Services.SystemLogs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CSO.Core.Repositories.UserRepo;

public class UserRepository : SqlTableRepository, IUserRepository
{
    private new readonly CSOResponsiveDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;
    public UserRepository(CSOResponsiveDbContext dbContext,
                              ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<User> Login(LoginViewModel loginViewModel)
    {
        try
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == loginViewModel.Username && x.Password == loginViewModel.Password);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<User> LoginWithAdId(string AdId)
    {
        try
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.ADid == AdId);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
    public async Task<UserViewModel?> GetUserByIdAsync(int userId)
    {
        try
        {
            var result = await _dbContext.Users
                .Where(i => i.Id == userId)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Designation = x.Designation,
                    Email = x.Email,
                    MobileNo = x.MobileNo,
                    RoleId = x.RoleId,
                    UserType = x.UserType,
                    DivisionId = x.DivisionId,
                    PlantId = x.PlantId,
                    NearestPlantId = x.NearestPlantId,
                    BrandId = x.BrandId,
                    ProductTypeId = x.ProductTypeId,
                    IsInMailMatrix = x.IsInMailMatrix,
                    ADid = x.ADid
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> InsertUserAsync(UserViewModel user)
    {
        try
        {
            var userDetails = new User
            {
                Name = user.Name,
                Designation = user.Designation,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId,
                UserType = user.UserType,
                DivisionId = user.DivisionId,
                PlantId = user.PlantId,
                NearestPlantId = user.NearestPlantId,
                BrandId = user.BrandId,
                ProductTypeId = user.ProductTypeId,
                IsInMailMatrix = user.IsInMailMatrix,
                ADid = user.ADid,
                UserName = user.ADid,
                Password = user.ADid,
                AddedBy = user.AddedBy,
                AddedOn = user.AddedOn,
                Rights = ""
            };

            var result = await base.CreateAsync<User>(userDetails);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateUserAsync(UserViewModel user)
    {
        try
        {
            var userDetails = await base.GetByIdAsync<User>(user.Id);
            if (userDetails == null)
                return new OperationResult
                {
                    Success = false,
                    Message = "No record found for this user to update."
                };

            userDetails.Name = user.Name;
            userDetails.Designation = user.Designation;
            userDetails.Email = user.Email;
            userDetails.MobileNo = user.MobileNo;
            userDetails.RoleId = user.RoleId;
            userDetails.UserType = user.UserType;
            userDetails.DivisionId = user.DivisionId;
            userDetails.PlantId = user.PlantId;
            userDetails.NearestPlantId = user.NearestPlantId;
            userDetails.BrandId = user.BrandId;
            userDetails.ProductTypeId = user.ProductTypeId;
            userDetails.IsInMailMatrix = user.IsInMailMatrix;
            userDetails.ADid = user.ADid;
            userDetails.UserName = user.ADid;
            userDetails.Password = user.ADid;
            userDetails.UpdatedBy = user.UpdatedBy;
            userDetails.UpdatedOn = user.UpdatedOn;

            var result = await base.UpdateAsync<User>(userDetails);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteUserAsync(int userId)
    {
        try
        {
            var result = await base.DeleteAsync<User>(userId);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<List<UsersGridModel>> GetAllUsersAsync()
    {
        try
        {
            var list = await _dbContext.Users
                .Join(_dbContext.UserRoles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) => new UsersGridModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        ADid = user.ADid,
                        Email = user.Email,
                        MobileNo = user.MobileNo,
                        Designation = user.Designation,
                        Role = role.RoleName
                    }
                )
                .OrderBy(o => o.Name)
                .ToListAsync();

            return list;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    //public async Task<TabulatorResult> GetAllUsersAsync(TabulatorRequest request)
    //{
    //    try
    //    {
    //        // Initial base query (before filters)
    //        var baseQuery = _dbContext.Users
    //            .Join(_dbContext.UserRoles,
    //                user => user.RoleId,
    //                role => role.Id,
    //                (user, role) => new UsersGridModel
    //                {
    //                    Id = user.Id,
    //                    Name = user.Name,
    //                    ADid = user.ADid,
    //                    Email = user.Email,
    //                    MobileNo = user.MobileNo,
    //                    Designation = user.Designation,
    //                    Role = role.RoleName
    //                })
    //            .AsQueryable();

    //        int totalRecords = await baseQuery.CountAsync(); // total before filter

    //        // Clone query for filtering
    //        var query = baseQuery;

    //        // Apply filters
    //        if (request.Filters != null)
    //        {
    //            foreach (var filter in request.Filters)
    //            {
    //                if (string.IsNullOrWhiteSpace(filter.Value)) continue;

    //                switch (filter.Field)
    //                {
    //                    case "Name":
    //                        query = query.Where(x => x.Name.Contains(filter.Value));
    //                        break;
    //                    case "ADid":
    //                        query = query.Where(x => x.ADid.Contains(filter.Value));
    //                        break;
    //                    case "Email":
    //                        query = query.Where(x => x.Email.Contains(filter.Value));
    //                        break;
    //                    case "MobileNo":
    //                        query = query.Where(x => x.MobileNo.Contains(filter.Value));
    //                        break;
    //                    case "Designation":
    //                        query = query.Where(x => x.Designation.Contains(filter.Value));
    //                        break;
    //                    case "Role":
    //                        query = query.Where(x => x.Role.Contains(filter.Value));
    //                        break;
    //                }
    //            }
    //        }

    //        int filteredRecords = await query.CountAsync(); // after filter

    //        // Apply sorting
    //        if (request.Sorters?.Any() == true)
    //        {
    //            var sorter = request.Sorters.First();
    //            var sortExpr = $"{sorter.Field} {(sorter.Dir == "asc" ? "ascending" : "descending")}";
    //            query = query.OrderBy(sortExpr); // Requires System.Linq.Dynamic.Core
    //        }
    //        else
    //        {
    //            // Default sort on Name
    //            query = query.OrderBy(x => x.Name);
    //        }

    //        // Pagination
    //        var data = await query
    //            .Skip((request.Page - 1) * request.Size)
    //            .Take(request.Size)
    //            .ToListAsync();

    //        // Assign serial numbers
    //        int srNo = ((request.Page - 1) * request.Size) + 1;
    //        for (int i = 0; i < data.Count; i++)
    //        {
    //            data[i].Sr_No = srNo + i;
    //        }

    //        return new TabulatorResult
    //        {
    //            LastPage = (int)Math.Ceiling((double)filteredRecords / request.Size),
    //            Data = data,
    //            TotalRecords = totalRecords,
    //            FilteredRecords = filteredRecords
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        _systemLogService.WriteLog(ex.Message);
    //        throw;
    //    }
    //}
}
