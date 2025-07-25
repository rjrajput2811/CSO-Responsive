using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("CSOLogsHistory")]
public class CSOLogHistory : SqlTable
{
    public int CSOLogId { get; set; }

    public int CSOLogBy { get; set; }

    public DateTime CSOLogOn { get; set; }

    public int? RootCauseBy { get; set; }

    public DateTime? RootCauseOn { get; set; }

    public int? MonitoringBy { get; set; }

    public DateTime? MonitoringOn { get; set; }

    public int? ReviewBy { get; set; }

    public DateTime? ReviewOn { get; set; }

    public int? CloseBy { get; set; }

    public DateTime? CloseOn { get; set; }

    [ForeignKey("CSOLogId")]
    public virtual CSOLog? CSOLogs { get; set; }
}
