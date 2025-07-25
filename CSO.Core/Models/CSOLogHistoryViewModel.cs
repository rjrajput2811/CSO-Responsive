namespace CSO.Core.Models;

public class CSOLogHistoryViewModel
{
    public int Id { get; set; }

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
}
