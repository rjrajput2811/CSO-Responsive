using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext.Shared;

public class SqlTable
{
    [Key]
    public virtual int Id { get; set; }
}

public class SqlTableOverrideExample : SqlTable
{
    //-----------------------------------
    // SqlTable override
    //-----------------------------------
    [Key]
    [Column("ID")]
    public override int Id { get; set; }
    //------------ END overrides --------
}
