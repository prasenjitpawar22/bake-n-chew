using BakeNChew.Data;
using BakeNChew.Models;
using BakeNChew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakeNChew.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<CartModel> Carts { get; set; }
    public DbSet<OrderModel> Orders { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        // builder.Entity<CartModel>().
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        //  builder.Entity<CartModel>()
        //     .HasRequired(x => x.AppUser)
        //     .WithMany()
        //     .Map(m => m.MapKey("UserId")); // Name of you FK column


        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}