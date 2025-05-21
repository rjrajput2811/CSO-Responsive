using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("ExceptionLogger")]
public class ExceptionLogger : SqlTable
{
    public string? ExceptionMessage { get; set; }
    public string? Type { get; set; }
    public string? ControllerName { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public DateTime LogTime { get; set; }
}
