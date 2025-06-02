namespace CSO.Core.Models;

public class BrandViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ProductTypeId { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DivisionId { get; set; }
    public string? ActiveInactive { get; set; }
}
