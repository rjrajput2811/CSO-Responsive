namespace CSO.Core.Models;

public class RecycleDayViewModel
{
    public int Id { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int ThresholdDays { get; set; }
    public int FinancialYear { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
