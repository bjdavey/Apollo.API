using API.Helpers;
using API.Shared;

namespace Apollo.API.Models.DB;

public partial class Vehicle : EntityBase
{
    public int? ProviderId { get; set; }

    public string? Title { get; set; }

    public string? Desc { get; set; }

    public string? Brand { get; set; }

    public int? Year { get; set; }

    public string? Color { get; set; }

    public VEHICLE_TYPE Type { get; set; }

    public string? Picture { get; set; }

    public PRICE_MODEL PriceModel { get; set; }

    public decimal? PricePerModel { get; set; }

    public VEHICLE_STATUS Status { get; set; }

    public string? Location { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Provider? Provider { get; set; }

    public virtual VehicleRestriction? VehicleRestriction { get; set; }
}
