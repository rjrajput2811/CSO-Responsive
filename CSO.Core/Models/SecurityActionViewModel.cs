namespace CSO.Core.Models;

public class SecurityActionViewModel
{
    public int Id { get; set; }
    public int UserRoleId { get; set; }
    public string UserRoleName { get; set; }
    public int Action { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
}
