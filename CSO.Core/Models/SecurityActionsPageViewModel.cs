namespace CSO.Core.Models;

public class SecurityActionsPageViewModel
{
    public int UserRoleId { get; set; }
    public string UserRoleName { get; set; }
    public List<SecurityActionViewModel> SecurityActionsViewModel { get; set; }
}
