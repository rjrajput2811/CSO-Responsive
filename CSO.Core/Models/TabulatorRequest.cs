using System.Text.Json.Serialization;

namespace CSO.Core.Models;

public class TabulatorRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
    public List<TabulatorSorter> Sorters { get; set; }
    public List<TabulatorFilter> Filters { get; set; }
}

public class TabulatorSorter
{
    public string Field { get; set; }
    public string Dir { get; set; }
}

public class TabulatorFilter
{
    public string Field { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}

public class TabulatorResult
{
    [JsonPropertyName("last_page")]
    public int LastPage { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }         // total before filtering (optional)

    [JsonPropertyName("filtered_records")]
    public int FilteredRecords { get; set; }      // total after filtering (optional)

    [JsonPropertyName("error")]
    public string Error { get; set; }
}

