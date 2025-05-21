using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("RecycleDay")]
public class RecycleDay : SqlTable
{
    //-----------------------------------
    // SqlTable override
    //-----------------------------------
    [Key]
    [Column("RecDayId")]
    public override int Id { get; set; }
    //------------ END overrides --------
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int ThresholdDays { get; set; }
    public int FinancialYear { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
