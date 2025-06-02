namespace CSO.Core.Models;

public class CSOLogFileViewModel
{
    public int Id { get; set; }

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
