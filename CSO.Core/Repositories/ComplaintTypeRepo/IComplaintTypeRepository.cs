using CSO.Core.DatabaseContext;
using CSO.Core.Models;

namespace CSO.Core.Repositories.ComplaintTypeRepo;

public interface IComplaintTypeRepository
{
    Task<List<ComplaintTypeViewModel>> GetComTypeList();
    Task<ComplaintType?> GetByIdAsync(int comTypeId);
    Task<OperationResult> CreateAsync(ComplaintType comType, bool returnCreatedRecord = false);
    Task<OperationResult> UpdateAsync(ComplaintType comType, bool returnUpdatedRecord = false);
    Task<OperationResult> DeleteAsync(int comTypeId);
    Task<bool> CheckDuplicate(string searchText, int Id);
}
