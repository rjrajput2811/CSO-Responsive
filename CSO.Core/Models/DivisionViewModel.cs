namespace CSO.Core.Models;

public class DivisionViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int AddedBy { get; set; }
    public string? AddedByUser { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }
    public string? UpdatedByUser { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }
}
