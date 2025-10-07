namespace CSO.Core.Models;

public class MSIReportByProductsViewModel
{
    // Primary Identifiers
    public int Id { get; set; }
    public string Name { get; set; }

    // --- Overall Counts ---
    public int CSO_Generated { get; set; }
    public int CSO_ClassA { get; set; }
    public int CSO_ClassB { get; set; }
    public string MajorCategory { get; set; }

    // --- Financial Year Counts ---
    public int CSO_Generated_YEAR { get; set; }
    public int CSO_ClassA_YEAR { get; set; }
    public int CSO_ClassB_YEAR { get; set; }
    public string MajorCategory_YEAR { get; set; }

    // --- Monthly Counts ---
    public int CSO_Generated_MONTH { get; set; }
    public int CSO_ClassA_MONTH { get; set; }
    public int CSO_ClassB_MONTH { get; set; }
    public string MajorCategory_MONTH { get; set; }

    // --- Quarterly Counts (QTLY) ---
    public int CSO_Generated_QTLY { get; set; }
    public int CSO_ClassA_QTLY { get; set; }
    public int CSO_ClassB_QTLY { get; set; }
    public string MajorCategory_QTLY { get; set; }
}
