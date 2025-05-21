using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("EmailConfiguration")]
public class EmailConfiguration : SqlTable
{
    public string? From { get; set; }
    public string? SmtpServer { get; set; }
    public int Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool SslRequired { get; set; }
    public string? DisplayName { get; set; }
    public string? OpenEmailSubject { get; set; }
    public string? OpenEmailBody { get; set; }
    public string? RootCauseEmailSubject { get; set; }
    public string? RootCauseEmailBody { get; set; }
    public string? MonitorEmailSubject { get; set; }
    public string? MonitorEmailBody { get; set; }
    public string? ApproveEmailSubject { get; set; }
    public string? ApproveEmailBody { get; set; }
    public string? RejectEmailSubject { get; set; }
    public string? RejectEmailBody { get; set; }
    public string? CloseEmailSubject { get; set; }
    public string? CloseEmailBody { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
}
