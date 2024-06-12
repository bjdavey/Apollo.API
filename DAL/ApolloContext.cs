using Apollo.API.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Apollo.API.DAL;

public partial class ApolloContext : DbContext
{
    public ApolloContext()
    {
    }

    public ApolloContext(DbContextOptions<ApolloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleRestriction> VehicleRestrictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .HasColumnName("address_line2");
            entity.Property(e => e.Birthdate)
                .HasColumnType("datetime")
                .HasColumnName("birthdate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.LicenseExpire)
                .HasColumnType("datetime")
                .HasColumnName("license_expire");
            entity.Property(e => e.LicenseIssue)
                .HasColumnType("datetime")
                .HasColumnName("license_issue");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("customer_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.CustomerId, "customer_id");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.HasIndex(e => e.VehicleId, "vehicle_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Details)
                .HasColumnType("json")
                .HasColumnName("details");
            entity.Property(e => e.EndAt)
                .HasColumnType("timestamp")
                .HasColumnName("end_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.PriceModel).HasColumnName("price_model");
            entity.Property(e => e.PricePerModel)
                .HasPrecision(15, 2)
                .HasColumnName("price_per_model");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.StartAt)
                .HasColumnType("timestamp")
                .HasColumnName("start_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalCost)
                .HasPrecision(15, 2)
                .HasColumnName("total_cost");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_ibfk_2");

            entity.HasOne(d => d.Provider).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_ibfk_1");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_ibfk_3");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("provider");

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cities)
                .HasColumnType("text")
                .HasColumnName("cities");
            entity.Property(e => e.ContractNo)
                .HasMaxLength(255)
                .HasColumnName("contract_no");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Documents)
                .HasColumnType("text")
                .HasColumnName("documents");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Logo)
                .HasColumnType("text")
                .HasColumnName("logo");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("started_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Provider)
                .HasForeignKey<Provider>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("provider_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Details)
                .HasColumnType("json")
                .HasColumnName("details");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Roles)
                .HasColumnType("text")
                .HasColumnName("roles");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehicle");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(255)
                .HasColumnName("brand");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Desc)
                .HasColumnType("text")
                .HasColumnName("desc");
            entity.Property(e => e.Details)
                .HasColumnType("json")
                .HasColumnName("details");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.DeviceUnique)
                .HasMaxLength(255)
                .HasColumnName("device_unique");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Picture)
                .HasColumnType("text")
                .HasColumnName("picture");
            entity.Property(e => e.PriceModel).HasColumnName("price_model");
            entity.Property(e => e.PricePerModel)
                .HasPrecision(15, 2)
                .HasColumnName("price_per_model");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Provider).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("vehicle_ibfk_1");
        });

        modelBuilder.Entity<VehicleRestriction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehicle_restriction");

            entity.HasIndex(e => e.VehicleId, "vehicle_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.MaxHours).HasColumnName("max_hours");
            entity.Property(e => e.MaxMile).HasColumnName("max_mile");
            entity.Property(e => e.MaxPersons).HasColumnName("max_persons");
            entity.Property(e => e.MinAge).HasColumnName("min_age");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Vehicle).WithOne(p => p.VehicleRestriction)
                .HasForeignKey<VehicleRestriction>(d => d.VehicleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("vehicle_restriction_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
