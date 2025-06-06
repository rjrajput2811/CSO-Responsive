using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("Users")]
public class User : SqlTable
{
    public string? Name { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }

    public string? MobileNo { get; set; }

    public int RoleId { get; set; }

    public string? Rights { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? DivisionId { get; set; }

    public string? PlantId { get; set; }

    public string? NearestPlantId { get; set; }

    public string? ProductTypeId { get; set; }

    public string? BrandId { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? ADid { get; set; }

    public int UserType { get; set; }

    public bool IsInMailMatrix { get; set; }
}
