using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("CSOLog")]
public class CSOLog : SqlTable
{
    public int UserId { get; set; }

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

    [ForeignKey("UserId")]
    public virtual User? Users { get; set; }

    [ForeignKey("DivisionId")]
    public virtual Division? Divisions { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Categorys? Categories { get; set; }

    [ForeignKey("ComplaintTypeId")]
    public virtual ComplaintType? ComplaintTypes { get; set; }

    [ForeignKey("CSOClassId")]
    public virtual CSOClass? CSOClasses { get; set; }

    [ForeignKey("BrandId")]
    public virtual Brand? Brands { get; set; }

    [ForeignKey("ProductTypeId")]
    public virtual ProductType? ProductTypes { get; set; }

    [ForeignKey("PlantId")]
    public virtual Plant? Plants { get; set; }

    [ForeignKey("NearestPlantId")]
    public virtual NearestPlant? NearestPlants { get; set; }

}
