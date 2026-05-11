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
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadImage> LeadImages => Set<LeadImage>();
    public DbSet<BuyerClient> BuyerClients => Set<BuyerClient>();
    public DbSet<PropertySale> PropertySales => Set<PropertySale>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<PropertyAmenity> PropertyAmenities => Set<PropertyAmenity>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<RequestRecord> Requests => Set<RequestRecord>();
    public DbSet<CommercialListing> CommercialListings => Set<CommercialListing>();
    public DbSet<ResidentialSeeker> ResidentialSeekers => Set<ResidentialSeeker>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO : Configure Relationships and Constraints
        
        modelBuilder.Entity<Contract>()
            .Property(c => c.MonthlyRent).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<PropertySale>()
            .Property(s => s.SalePrice).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Lead>()
            .Property(l => l.ListedPrice).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Lead>()
            .Property(l => l.CommissionAmount).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Property>()
            .Property(p => p.SalePrice).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Property>()
            .Property(p => p.RentPrice).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<RequestRecord>()
            .Property(r => r.MaxBudget).HasColumnType("numeric(18,2)");

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

        modelBuilder.Entity<User>()
            .Property(u => u.ScreenPermissionsJson)
            .IsRequired()
            .HasColumnType("text")
            .HasDefaultValue("[]");

        modelBuilder.Entity<RequestRecord>()
            .Property(r => r.FullName).IsRequired();
        modelBuilder.Entity<RequestRecord>()
            .Property(r => r.MobileNumber).IsRequired();
        modelBuilder.Entity<RequestRecord>()
            .Property(r => r.RequestType).IsRequired();

        modelBuilder.Entity<CommercialListing>(entity =>
        {
            entity.ToTable("commercial_listings");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RowFlag).HasColumnName("row_flag").HasColumnType("text");
            entity.Property(e => e.SerialNumber).HasColumnName("serial_number").HasColumnType("text");
            entity.Property(e => e.ContactDate).HasColumnName("contact_date").HasColumnType("text");
            entity.Property(e => e.PropertyStatus).HasColumnName("property_status").HasColumnType("text");
            entity.Property(e => e.BrokerageContract).HasColumnName("brokerage_contract").HasColumnType("text");
            entity.Property(e => e.LicenseNumber).HasColumnName("license_number").HasColumnType("text");
            entity.Property(e => e.ContractExpiry).HasColumnName("contract_expiry").HasColumnType("text");
            entity.Property(e => e.AdNumber).HasColumnName("ad_number").HasColumnType("text");
            entity.Property(e => e.Employee).HasColumnName("employee").HasColumnType("text");
            entity.Property(e => e.Broker).HasColumnName("broker").HasColumnType("text");
            entity.Property(e => e.OwnerName).HasColumnName("owner_name").HasColumnType("text");
            entity.Property(e => e.Mobile1).HasColumnName("mobile1").HasColumnType("text");
            entity.Property(e => e.Mobile2).HasColumnName("mobile2").HasColumnType("text");
            entity.Property(e => e.AvailableUnits).HasColumnName("available_units").HasColumnType("text");
            entity.Property(e => e.DeedNumber).HasColumnName("deed_number").HasColumnType("text");
            entity.Property(e => e.PropertyType).HasColumnName("property_type").HasColumnType("text");
            entity.Property(e => e.RoomsCount).HasColumnName("rooms_count").HasColumnType("text");
            entity.Property(e => e.BuildingAge).HasColumnName("building_age").HasColumnType("text");
            entity.Property(e => e.HasElevator).HasColumnName("has_elevator").HasColumnType("text");
            entity.Property(e => e.OtherDetails).HasColumnName("other_details").HasColumnType("text");
            entity.Property(e => e.RentAmount).HasColumnName("rent_amount").HasColumnType("text");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type").HasColumnType("text");
            entity.Property(e => e.Location).HasColumnName("location").HasColumnType("text");
            entity.Property(e => e.Coordinates).HasColumnName("coordinates").HasColumnType("text");
            entity.Property(e => e.HasKey).HasColumnName("has_key").HasColumnType("text");
            entity.Property(e => e.PublishedTahmid).HasColumnName("published_tahmid").HasColumnType("text");
            entity.Property(e => e.PublishedBoard).HasColumnName("published_board").HasColumnType("text");
            entity.Property(e => e.PublishedDesigns).HasColumnName("published_designs").HasColumnType("text");
            entity.Property(e => e.PublishedHaraj).HasColumnName("published_haraj").HasColumnType("text");
            entity.Property(e => e.PublishedDeal).HasColumnName("published_deal").HasColumnType("text");
            entity.Property(e => e.PublishedAqar).HasColumnName("published_aqar").HasColumnType("text");
            entity.Property(e => e.PublishedBayut).HasColumnName("published_bayut").HasColumnType("text");
            entity.Property(e => e.PublishedDhaki).HasColumnName("published_dhaki").HasColumnType("text");
            entity.Property(e => e.PublishedWhatsapp).HasColumnName("published_whatsapp").HasColumnType("text");
            entity.Property(e => e.PublishedTwitter).HasColumnName("published_twitter").HasColumnType("text");
            entity.Property(e => e.PublishedWhatsappGroup).HasColumnName("published_whatsapp_group").HasColumnType("text");
            entity.Property(e => e.PublishedWhatsappChannel).HasColumnName("published_whatsapp_channel").HasColumnType("text");
            entity.Property(e => e.PublishedSnapchat).HasColumnName("published_snapchat").HasColumnType("text");
            entity.Property(e => e.PublishedX).HasColumnName("published_x").HasColumnType("text");
            entity.Property(e => e.PublishedInstagram).HasColumnName("published_instagram").HasColumnType("text");
            entity.Property(e => e.PublishedTiktok).HasColumnName("published_tiktok").HasColumnType("text");
            entity.Property(e => e.Notes).HasColumnName("notes").HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<ResidentialSeeker>(entity =>
        {
            entity.ToTable("residential_seekers");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SerialNumber).HasColumnName("serial_number").HasColumnType("text");
            entity.Property(e => e.RequestDate).HasColumnName("request_date").HasColumnType("text");
            entity.Property(e => e.Status).HasColumnName("status").HasColumnType("text");
            entity.Property(e => e.Employee).HasColumnName("employee").HasColumnType("text");
            entity.Property(e => e.Receiver).HasColumnName("receiver").HasColumnType("text");
            entity.Property(e => e.SourceChannel).HasColumnName("source_channel").HasColumnType("text");
            entity.Property(e => e.Mobile).HasColumnName("mobile").HasColumnType("text");
            entity.Property(e => e.FullName).HasColumnName("full_name").HasColumnType("text");
            entity.Property(e => e.Nationality).HasColumnName("nationality").HasColumnType("text");
            entity.Property(e => e.Profession).HasColumnName("profession").HasColumnType("text");
            entity.Property(e => e.FamilyCount).HasColumnName("family_count").HasColumnType("text");
            entity.Property(e => e.RequestDescription).HasColumnName("request_description").HasColumnType("text");
            entity.Property(e => e.MaxBudget).HasColumnName("max_budget").HasColumnType("text");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type").HasColumnType("text");
            entity.Property(e => e.PreferredLocation).HasColumnName("preferred_location").HasColumnType("text");
            entity.Property(e => e.Notes).HasColumnName("notes").HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        });

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

        modelBuilder.Entity<LeadImage>()
            .Property(i => i.StoredFileName).IsRequired();
        modelBuilder.Entity<LeadImage>()
            .Property(i => i.OriginalFileName).IsRequired();
        modelBuilder.Entity<LeadImage>()
            .Property(i => i.RelativePath).IsRequired();
        modelBuilder.Entity<LeadImage>()
            .Property(i => i.MimeType).IsRequired();
        modelBuilder.Entity<LeadImage>()
            .HasIndex(i => i.LeadId);
        modelBuilder.Entity<LeadImage>()
            .HasIndex(i => new { i.LeadId, i.SortOrder });

        modelBuilder.Entity<Tenant>()
            .HasOne(t => t.Property)
            .WithMany()
            .HasForeignKey(t => t.PropertyId)
            .OnDelete(DeleteBehavior.SetNull);

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

        modelBuilder.Entity<Partner>()
            .ToTable("partners");
        modelBuilder.Entity<Partner>()
            .Property(p => p.FullName)
            .IsRequired();
        modelBuilder.Entity<Partner>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Partner>(p => p.UserId);
        modelBuilder.Entity<Partner>()
            .HasIndex(p => p.UserId)
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
        modelBuilder.Entity<Lead>()
            .HasOne(l => l.Partner)
            .WithMany(p => p.Leads)
            .HasForeignKey(l => l.PartnerId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LeadImage>()
            .HasOne(i => i.Lead)
            .WithMany(l => l.Images)
            .HasForeignKey(i => i.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
