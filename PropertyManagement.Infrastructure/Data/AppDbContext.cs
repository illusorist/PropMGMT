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
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<BuyerClient> BuyerClients => Set<BuyerClient>();
    public DbSet<PropertySale> PropertySales => Set<PropertySale>();
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
        modelBuilder.Entity<PropertySale>()
            .Property(s => s.SalePrice).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount).HasColumnType("numeric(18,2)");

        modelBuilder.Entity<Contract>()
            .Property(c => c.Status).HasConversion<string>();
        modelBuilder.Entity<Property>()
            .Property(p => p.Status)
            .HasConversion<string>()
            .HasDefaultValue(PropertyStatus.Pending);
        modelBuilder.Entity<Lead>()
            .Property(l => l.Intent)
            .HasConversion<string>();
        modelBuilder.Entity<Lead>()
            .Property(l => l.Status)
            .HasConversion<string>()
            .HasDefaultValue(LeadStatus.New);
        modelBuilder.Entity<Payment>()
            .Property(p => p.Status).HasConversion<string>();

        modelBuilder.Entity<Amenity>()
            .Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Amenity>()
            .Property(a => a.NormalizedName).IsRequired();
        modelBuilder.Entity<Amenity>()
            .HasIndex(a => a.NormalizedName).IsUnique();

        modelBuilder.Entity<PropertyImage>()
            .Property(i => i.StoredFileName).IsRequired();
        modelBuilder.Entity<PropertyImage>()
            .Property(i => i.OriginalFileName).IsRequired();
        modelBuilder.Entity<PropertyImage>()
            .Property(i => i.RelativePath).IsRequired();
        modelBuilder.Entity<PropertyImage>()
            .Property(i => i.MimeType).IsRequired();
        modelBuilder.Entity<PropertyImage>()
            .HasIndex(i => i.PropertyId);
        modelBuilder.Entity<PropertyImage>()
            .HasIndex(i => new { i.PropertyId, i.SortOrder });
        modelBuilder.Entity<PropertyImage>()
            .HasIndex(i => i.IsPrimary)
            .HasFilter("\"IsPrimary\" = true");

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

        modelBuilder.Entity<PropertySale>()
            .HasOne(s => s.Property)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.PropertyId);
        modelBuilder.Entity<PropertySale>()
            .HasOne(s => s.BuyerClient)
            .WithMany(b => b.Sales)
            .HasForeignKey(s => s.BuyerClientId);

        modelBuilder.Entity<PropertyImage>()
            .HasOne(i => i.Property)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Lead>()
            .HasOne(l => l.Property)
            .WithMany(p => p.Leads)
            .HasForeignKey(l => l.PropertyId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Lead>()
            .HasOne(l => l.AssignedToUser)
            .WithMany()
            .HasForeignKey(l => l.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);

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
