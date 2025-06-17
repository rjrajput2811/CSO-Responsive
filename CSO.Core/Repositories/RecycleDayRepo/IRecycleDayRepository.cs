using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.RecycleDayRepo;

public interface IRecycleDayRepository
{
    Task<List<RecycleDayViewModel>> GetRecycleDayList();
    Task<RecycleDay?> GetByIdAsync(int recycleDayId);
    Task<OperationResult> CreateAsync(RecycleDay recycleDay, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(RecycleDay recycleDay, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int recycleDayId);

    //Task<bool> CheckDuplicate(string searchText, int Id);
}
