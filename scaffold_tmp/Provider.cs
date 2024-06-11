using System;
using System.Collections.Generic;

namespace Apollo.API.scaffold_tmp;

public partial class Provider
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? ContractNo { get; set; }

    public string? Logo { get; set; }

    public string? Documents { get; set; }

    public string? Cities { get; set; }

    public DateTime? StartedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
