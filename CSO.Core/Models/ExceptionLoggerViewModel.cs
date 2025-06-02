namespace CSO.Core.Models;

public class ExceptionLoggerViewModel
{
    public int Id { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? Type { get; set; }
    public string? ControllerName { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public DateTime LogTime { get; set; }
}
