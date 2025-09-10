using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Models;

public class DashboardViewModel
{
    public int AllCount { get; set; }
    public int OpenCount { get; set; }
    public int RootCausedCount { get; set; }
    public int MonitoredCount { get; set; }
    public int ApprovedCount { get; set; }
    public int ClosedCount { get; set; }
    public int April_AllCount { get; set; }
    public int May_AllCount { get; set; }
    public int June_AllCount { get; set; }
    public int July_AllCount { get; set; }
    public int August_AllCount { get; set; }
    public int September_AllCount { get; set; }
    public int October_AllCount { get; set; }
    public int November_AllCount { get; set; }
    public int December_AllCount { get; set; }
    public int January_AllCount { get; set; }
    public int February_AllCount { get; set; }
    public int March_AllCount { get; set; }
    public int RootCauseOverdueCount { get; set; }
    public int MonitorOverdueCount { get; set; }
    public int ApproveOverdueCount { get; set; }
    public int ClosureOverdueCount { get; set; }
    public int RootCauseThresholdDays { get; set; }
    public int MonitorThresholdDays { get; set; }
    public int ApproveThresholdDays { get; set; }
    public int ClosureThresholdDays { get; set; }
    public int OverallOpenCount { get; set; }
    public int OverallClosedCount { get; set; }
    public List<PlantData>? PlantData { get; set; }
}

public class PlantData
{
    public string? PlantName { get; set; }
    public int AllCount { get; set; }
}
