namespace CSO.Core.Models;

public class UsersRoleViewModel
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public string? Permission { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
