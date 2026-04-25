using System;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<PropertyAmenity> PropertyAmenities => Set<PropertyAmenity>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Payment> Payments => Set<Payment>();

    //TODO : Delete this
    //    string hash = BCrypt.Net.BCrypt.HashPassword("admin123");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO : Configure Relationships and Constraints
        
        modelBuilder.Entity<Contract>()
            .Property(c => c.MonthlyRent).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount).HasColumnType("numeric(18,2)");

        modelBuilder.Entity<Contract>()
            .Property(c => c.Status).HasConversion<string>();
        modelBuilder.Entity<Property>()
            .Property(p => p.Status)
            .HasConversion<string>()
            .HasDefaultValue(PropertyStatus.Pending);
        modelBuilder.Entity<Payment>()
            .Property(p => p.Status).HasConversion<string>();

        modelBuilder.Entity<Amenity>()
            .Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Amenity>()
            .Property(a => a.NormalizedName).IsRequired();
        modelBuilder.Entity<Amenity>()
            .HasIndex(a => a.NormalizedName).IsUnique();

        modelBuilder.Entity<PropertyAmenity>()
            .HasKey(pa => new { pa.PropertyId, pa.AmenityId });
        modelBuilder.Entity<PropertyAmenity>()
            .HasOne(pa => pa.Property)
            .WithMany(p => p.PropertyAmenities)
            .HasForeignKey(pa => pa.PropertyId);
        modelBuilder.Entity<PropertyAmenity>()
            .HasOne(pa => pa.Amenity)
            .WithMany(a => a.PropertyAmenities)
            .HasForeignKey(pa => pa.AmenityId);

        modelBuilder.Entity<Owner>()
            .HasOne(o => o.User)
            .WithOne()
            .HasForeignKey<Owner>(o => o.UserId);
        modelBuilder.Entity<Owner>()
            .HasIndex(o => o.UserId)
            .IsUnique();

        modelBuilder.Entity<User>().HasData(new User
        {
            // TODO : Change this password hash to a more secure one before production
            // Or really consider implementing a proper user management system with registration, password reset, etc.
            
            Id = 1,
            Username = "admin",
            PasswordHash = "$2a$12$zxxAdgPh00F5tMWopmrSxebIlwof7p/rFcfDcjhMoM0UaLey9Mr5q",
            Role = "Admin",
            CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 4, 20, 0, 0, 0), DateTimeKind.Utc)
        });
    }
}
