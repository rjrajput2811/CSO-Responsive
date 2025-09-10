using CSO.Core.Models;

namespace CSO.Core.Services.DashboardService;

public interface IDashboardService
{
    Task<DashboardViewModel?> GetDashboardDataAsync(int fYear, int userId, string divisionIds, string plantIds, DateTime? fromDate, DateTime? toDate);
}
