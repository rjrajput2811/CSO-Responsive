namespace CSO.Core.Models;

public class CSOLogAnalysisSolutionFilter
{
    public int? PendingDays { get; set; }
    public int? Status { get; set; }
    public string? PlantId { get; set; }
    public string? DivisionId { get; set; }
    public string? NearestPlantId { get; set; }
    public string? ProductTypeId { get; set; }
    public string? BrandId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int CrossDays { get; set; }
    public int NotClosed { get; set; }
    public bool ViewMyCSO { get; set; }
    public bool ViewAllCSO { get; set; }
    public string? SearchPlantId { get; set; }
    public int RoleId { get; set; }
    public int UserType { get; set; }
    public int FinancialYear { get; set; }
    public string? CSONoSearch { get; set; }
    public int UserId { get; set; }
}
