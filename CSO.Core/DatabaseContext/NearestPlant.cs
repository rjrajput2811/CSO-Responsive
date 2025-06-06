using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("NearestPlants")]
public class NearestPlant : SqlTable
{
    public string? Name { get; set; }

    public string? PlantId { get; set; }

    public int AddedBy { get; set; }

    public DateTime AddedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }
}
