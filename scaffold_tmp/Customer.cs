using System;
using System.Collections.Generic;

namespace Apollo.API.scaffold_tmp;

public partial class Customer
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public DateTime? LicenseIssue { get; set; }

    public DateTime? LicenseExpire { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
