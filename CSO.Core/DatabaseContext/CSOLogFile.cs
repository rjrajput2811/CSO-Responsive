using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("CSOLogFiles")]
public class CSOLogFile : SqlTable
{
    public string? FilePath { get; set; }

    public string? FileName { get; set; }

    public int Type { get; set; }

    public int CSOLogId { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }
}
