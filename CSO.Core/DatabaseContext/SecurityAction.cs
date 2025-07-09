using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("SecurityActions")]
public class SecurityAction : SqlTable
{
    //-----------------------------------
    // SqlTable override
    //-----------------------------------
    [Key]
    [Column("ID")]
    public override int Id { get; set; }
    //------------ END overrides --------

    public int UserRoleId { get; set; }
    public int Action { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
}
