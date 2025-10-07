namespace CSO.Core.Models;

public class MSIReportByPlantViewModel
{
    // Primary Identifiers
    public int Id { get; set; }
    public string Name { get; set; }

    // --- Overall Counts ---
    public int CSO_Generated { get; set; }
    public int CSO_Closed { get; set; }
    public int CSO_ClassOne { get; set; }

    // --- Overall Phase-wise Overdue Counts ---
    public int RootCauseOverdue { get; set; }
    public int MonitorOverdue { get; set; }
    public int ApproveOverdue { get; set; }
    public int ClosureOverdue { get; set; }

    // --- Financial Year Counts ---
    public int CSO_Generated_YEAR { get; set; }
    public int CSO_Closed_YEAR { get; set; }
    public int CSO_ClassOne_YEAR { get; set; }

    // --- Financial Year Phase-wise Overdue Counts ---
    public int RootCauseOverdue_YEAR { get; set; }
    public int MonitorOverdue_YEAR { get; set; }
    public int ApproveOverdue_YEAR { get; set; }
    public int ClosureOverdue_YEAR { get; set; }

    // --- Monthly Counts ---
    public int CSO_Generated_MONTH { get; set; }
    public int CSO_Closed_Month { get; set; } // Matches the exact SQL alias casing
    public int CSO_ClassOne_MONTH { get; set; }

    // --- Monthly Phase-wise Overdue Counts ---
    public int RootCauseOverdue_MONTH { get; set; }
    public int MonitorOverdue_MONTH { get; set; }
    public int ApproveOverdue_MONTH { get; set; }
    public int ClosureOverdue_MONTH { get; set; }

    // --- Quarterly Counts (QTLY) ---
    public int CSO_Generated_QTLY { get; set; }
    public int CSO_Closed_QTLY { get; set; }
    public int CSO_ClassOne_QTLY { get; set; }

    // --- Quarterly Phase-wise Overdue Counts (QTLY) ---
    public int RootCauseOverdue_QTLY { get; set; }
    public int MonitorOverdue_QTLY { get; set; }
    public int ApproveOverdue_QTLY { get; set; }
    public int ClosureOverdue_QTLY { get; set; }
}
