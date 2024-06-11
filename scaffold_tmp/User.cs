using System;
using System.Collections.Generic;

namespace Apollo.API.scaffold_tmp;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public sbyte? Status { get; set; }

    public sbyte? Type { get; set; }

    public string? Roles { get; set; }

    public string? Details { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Provider? Provider { get; set; }
}
