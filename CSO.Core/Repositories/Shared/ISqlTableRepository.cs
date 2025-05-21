using CSO.Core.DatabaseContext.Shared;
using CSO.Core.Models;

namespace CSO.Core.Repositories.Shared;

public interface ISqlTableRepository
{
    Task<OperationResult> CreateAsync<TEntity>(SqlTable record, bool returnCreatedRecord = false) where TEntity : SqlTable;
    Task<OperationResult> UpdateAsync<TEntity>(SqlTable modifiedRecord, bool returnUpdatedRecord = false) where TEntity : SqlTable;
    Task<OperationResult> DeleteAsync<TEntity>(int id) where TEntity : SqlTable;
    Task<TEntity?> GetByIdAsync<TEntity>(int id) where TEntity : SqlTable;
}
