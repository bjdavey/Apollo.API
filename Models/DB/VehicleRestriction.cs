using API.Helpers;

namespace Apollo.API.Models.DB;

public partial class VehicleRestriction : EntityBase
{
    public int? VehicleId { get; set; }

    public long? MaxHours { get; set; }

    public long? MaxMile { get; set; }

    public int? MinAge { get; set; }

    public int? MaxPersons { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
