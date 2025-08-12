using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.RecycleDayRepo;

public interface IRecycleDayRepository
{
    Task<List<RecycleDayViewModel>> GetRecycleDayList();
    Task<RecycleDayViewModel?> GetByIdAsync(int recycleDayId);
    Task<OperationResult> CreateAsync(RecycleDayViewModel recycleDay, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(RecycleDayViewModel recycleDay, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int recycleDayId);
    Task<bool> IsDateRangeOverlapping(int recDayId, int csoLogPhase, int financialYear, DateTime fromDate, DateTime toDate);
    Task<bool> IsDateUsedInLogCso(DateTime fromDate, DateTime toDate, int csoLogPhase);

    //Task<bool> CheckDuplicate(string searchText, int Id);
}
