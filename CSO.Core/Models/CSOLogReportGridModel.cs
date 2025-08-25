using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Models;

public class CSOLogReportGridModel
{
    // Fields from CSOLogs and other tables
    public string? CSONo { get; set; }
    public int Status1 { get; set; }
    public string? FinancialYear { get; set; }
    public int PlantId { get; set; }
    public int DivisionId { get; set; }
    public int BrandId { get; set; }
    public int ProductTypeId { get; set; }
    public int? NearestPlantId { get; set; }
    public DateTime? Logdate { get; set; }
    public int AddedBy { get; set; }
    public string? Division { get; set; }
    public string? Plant { get; set; }
    public string? Brand { get; set; }
    public string? ProductType { get; set; }
    public string? NearestPlant { get; set; }
    public string? ComplaintTypeName { get; set; }
    public string? ClassName { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public string? SourceofComplaint { get; set; }
    public string? Batch { get; set; }
    public int Quantity { get; set; }
    public int SuppliedQuantity { get; set; }
    public string? CatReference { get; set; }
    public bool IsSampleShipped { get; set; }
    public string? TrackingNo { get; set; }
    public string? CorrectiveActionDescription { get; set; }
    public string? MonitoringofCorrectiveActionDescription { get; set; }
    public string? PreventiveActionDescription { get; set; }
    public string? RootCauseAnalysisDescription { get; set; }
    public string? SKUDetails { get; set; }
    public string? PKDDate { get; set; }
    public string? Review1 { get; set; }
    public string? Review2 { get; set; }

    // Fields from CSOLogsHistory
    public int CSOLogId { get; set; }
    public DateTime? CSOLogOn { get; set; }
    public DateTime? RootCauseOn { get; set; }
    public DateTime? MonitoringOn { get; set; }
    public DateTime? ReviewOn { get; set; }
    public DateTime? CloseOn { get; set; }

    // User names
    public string? CSOLogUserName { get; set; }
    public string? RootCauseUserName { get; set; }
    public string? MonitorUserName { get; set; }
    public string? ApproveUserName { get; set; }
    public string? CloseUserName { get; set; }

    // New status and day calculation properties
    public string? RootStatus { get; set; }
    public int RootCompletedInDays { get; set; }
    public int RootPendingDays { get; set; }

    public string? MonitorStatus { get; set; }
    public int MonitorCompletedInDays { get; set; }
    public int MonitorPendingDays { get; set; }

    public string? ApprovalStatus { get; set; }
    public int ApprovalCompletedInDays { get; set; }
    public int ApprovalPendingDays { get; set; }

    public string? ClosureStatus { get; set; }
    public int ClosureCompletedInDays { get; set; }
    public int ClosurePendingDays { get; set; }
}
