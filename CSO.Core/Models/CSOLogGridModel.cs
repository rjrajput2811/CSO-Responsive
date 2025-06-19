namespace CSO.Core.Models;

public class CSOLogGridModel
{
    public int Id { get; set; }

    public int CSONo { get; set; }

    public string? UserName { get; set; }

    public DateTime? Logdate { get; set; }

    public string? DivisionName { get; set; }

    public string? CategoryName { get; set; }

    public string? ComplaintTypeName { get; set; }

    public string? Description { get; set; }

    public string? SourceofComplaint { get; set; }

    public string? CSOClassName { get; set; }

    public string? BrandName { get; set; }

    public string? ProductTypeName { get; set; }

    public string? PlantName { get; set; }

    public string? NearestPlantName { get; set; }

    public string? Batch { get; set; }

    public string? Date { get; set; }

    public int PendingDays { get; set; }

    public int Quantity { get; set; }

    public string? Status { get; set; }

    //Newly added 
    public int DaysCompleted { get; set; }

    public string? CSONoFYear { get; set; }

    public string? ApproverRemarks { get; set; }
    public string? ClosureRemarks { get; set; }
}
