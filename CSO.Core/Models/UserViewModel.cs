using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace CSO.Core.Models;

public class UserViewModel
{
    public int Id { get; set; }

    [DisplayName("Name")]
    public string? Name { get; set; }

    [DisplayName("Designation")]
    public string? Designation { get; set; }

    [DisplayName("Email")]
    public string? Email { get; set; }

    [DisplayName("Mobile No.")]
    public string? MobileNo { get; set; }

    [DisplayName("Role")]
    public int RoleId { get; set; }

    public string? Rights { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    [DisplayName("Division")]
    public string? DivisionId { get; set; }

    [DisplayName("Plant")]
    public string? PlantId { get; set; }

    [DisplayName("Nearest Plant")]
    public string? NearestPlantId { get; set; }

    [DisplayName("Product Type")]
    public string? ProductTypeId { get; set; }

    [DisplayName("Brand")]
    public string? BrandId { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    [DisplayName("ADid")]
    public string? ADid { get; set; }

    [DisplayName("User Type")]
    public int UserType { get; set; }

    [DisplayName("Is In Mail Matrix")]
    public bool IsInMailMatrix { get; set; }
}
