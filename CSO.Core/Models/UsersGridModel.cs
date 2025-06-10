using System.ComponentModel;

namespace CSO.Core.Models;

public class UsersGridModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Designation { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public int RoleId { get; set; }
    public string? Role { get; set; }
    public string? ADid { get; set; }
}
