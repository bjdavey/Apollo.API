using System;
using System.Collections.Generic;

namespace Apollo.API.scaffold_tmp;

public partial class Order
{
    public int Id { get; set; }

    public int? ProviderId { get; set; }

    public int? CustomerId { get; set; }

    public int? VehicleId { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public sbyte? PriceModel { get; set; }

    public decimal? PricePerModel { get; set; }

    public decimal? TotalCost { get; set; }

    public sbyte? Status { get; set; }

    public string? Location { get; set; }

    public string? Details { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Provider? Provider { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
