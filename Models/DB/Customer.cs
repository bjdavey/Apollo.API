using API.Helpers;

namespace Apollo.API.Models.DB;

public partial class Customer : EntityBase
{
    public int? UserId { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public DateTime? LicenseIssue { get; set; }

    public DateTime? LicenseExpire { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
