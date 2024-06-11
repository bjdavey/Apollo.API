using API.Helpers;
using API.Shared;

namespace Apollo.API.Models.DB;

public partial class Order : EntityBase
{
    public int? ProviderId { get; set; }

    public int? CustomerId { get; set; }

    public int? VehicleId { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public PRICE_MODEL PriceModel { get; set; }

    public decimal? PricePerModel { get; set; }

    public decimal? TotalCost { get; set; }

    public ORDER_STATUS Status { get; set; }

    public string? Location { get; set; }

    public string? Details { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Provider? Provider { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
