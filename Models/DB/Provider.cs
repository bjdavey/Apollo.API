using API.Helpers;

namespace Apollo.API.Models.DB;

public partial class Provider : EntityBase
{
    public int? UserId { get; set; }

    public string? ContractNo { get; set; }

    public string? Logo { get; set; }

    public string? Documents { get; set; }

    public string? Cities { get; set; }

    public DateTime? StartedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
