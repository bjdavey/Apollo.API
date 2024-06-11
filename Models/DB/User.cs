using API.Helpers;

namespace Apollo.API.Models.DB;

public partial class User : EntityBase
{
    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public sbyte? Status { get; set; }

    public sbyte? Type { get; set; }

    public string? Roles { get; set; }

    public string? Details { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Provider? Provider { get; set; }
}
