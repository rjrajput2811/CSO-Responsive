using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.Models;

public class CSOLogViewModel
{
    public int Id { get; set; }

    public int CSONo { get; set; }

    public int UserId { get; set; }

    public string? UserName { get; set; }

    public DateTime Logdate { get; set; }

    public int DivisionId { get; set; }

    public int CategoryId { get; set; }

    public System.Nullable<int> ComplaintTypeId { get; set; }

    public string? Description { get; set; }

    public string? SourceofComplaint { get; set; }

    public System.Nullable<int> CSOClassId { get; set; }

    public int BrandId { get; set; }

    public int ProductTypeId { get; set; }

    public int PlantId { get; set; }

    public int NearestPlantId { get; set; }

    public string? Batch { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public string? CatReference { get; set; }

    public bool IsSampleShipped { get; set; }

    public string? TrackingNo { get; set; }

    public int SuppliedQuantity { get; set; }

    public int Status1 { get; set; }

    public string? Review1 { get; set; }

    public int? Status2 { get; set; }

    public string? Review2 { get; set; }

    public string? RootCauseAnalysisDescription { get; set; }

    public string? CorrectiveActionDescription { get; set; }

    public string? PreventiveActionDescription { get; set; }

    public string? MonitoringofCorrectiveActionDescription { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int FinancialYear { get; set; }

    public string? SKUDetails { get; set; }

    public string? PKDDate { get; set; }

    public string? CSONoFYear { get; set; }

    public string? PlantName { get; set; }
    public string? BrandName { get; set; }
    public string? ProductTypeName { get; set; }
    public string? ComplainTypeName { get; set; }

}
