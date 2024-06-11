using System;
using System.Collections.Generic;

namespace Apollo.API.scaffold_tmp;

public partial class VehicleRestriction
{
    public int Id { get; set; }

    public int? VehicleId { get; set; }

    public long? MaxHours { get; set; }

    public long? MaxMile { get; set; }

    public int? MinAge { get; set; }

    public int? MaxPersons { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
