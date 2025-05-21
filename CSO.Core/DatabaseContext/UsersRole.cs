using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("UsersRole")]
public class UsersRole : SqlTable
{
    //-----------------------------------
    // SqlTable override
    //-----------------------------------
    [Key]
    [Column("RoleId")]
    public override int Id { get; set; }
    //------------ END overrides --------
    public string? RoleName { get; set; }
    public string? Permission { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
