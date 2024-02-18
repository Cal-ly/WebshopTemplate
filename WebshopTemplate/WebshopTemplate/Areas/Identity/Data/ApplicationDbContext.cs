using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Models;

namespace WebshopTemplate.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Entity Framework will handle the inheritance hierarchy by default
    /// using the Table-Per-Hierarchy (TPH) approach, where a single table
    /// will contain users of all types, distinguished by a discriminator column.
    /// </summary>
    //public DbSet<IdentityUser> Users { get; set; }
    public DbSet<Staff> StaffMembers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Relationship IdentityUser
        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.Id);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });
        #endregion

        #region Relationship Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.HasIndex(c => c.Id);
            entity.HasOne(c => c.User).WithOne().HasForeignKey<Customer>(c => c.UserId); // One-to-One relationship between Customer and IdentityUser

            entity.HasOne(c => c.RepresentingCompany)
                        .WithMany(co => co.Representatives)
                                        .HasForeignKey(c => c.CompanyId);
            entity.Property(c => c.CompanyId)
                        .IsRequired(false);
            entity.Property(c => c.CompanyId)
                        .HasDefaultValue(null);
            entity.Property(c => c.CompanyId)
                        .ValueGeneratedNever();
        });
        #endregion

        #region Relationship Staff
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("StaffMembers");
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.HasIndex(s => s.Id);
            entity.HasOne(s => s.User).WithOne().HasForeignKey<Staff>(s => s.UserId); // One-to-One relationship between Staff and IdentityUser

            entity.HasMany(s => s.Orders) // Staff has many Orders
                    .WithOne(o => o.Staff) // Each Order has one Staff
                        .HasForeignKey(o => o.StaffId); // ForeignKey in Order pointing to Staff
        });
        #endregion

        #region Relationship Order - OrderDetail - Product
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderDetails) // Order has many OrderDetails
            .WithOne(od => od.Order) // Each OrderDetail has one Order
            .HasForeignKey(od => od.OrderId); // ForeignKey in OrderDetail pointing to Order
        modelBuilder.Entity<Order>()
            .HasIndex(o => o.Id); // Index on Id in Order
        #endregion

        #region Relationship OrderDetail - Product
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product) // Each OrderDetail has one Product
            .WithMany(p => p.OrderDetails) // Product has many OrderDetails
            .HasForeignKey(od => od.ProductId); // ForeignKey in OrderDetail pointing to Product
        modelBuilder.Entity<OrderDetail>()
            .HasIndex(od => od.ProductId); // Index on ProductId in OrderDetail
        #endregion

        #region Relationship Product - Category
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category) // Each Product has one Category
            .WithMany(c => c.Products) // Category has many Products
            .HasForeignKey(p => p.CategoryId); // ForeignKey in Product pointing to Category
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.CategoryId); // Index on CategoryId in Product
        #endregion

    }

public DbSet<WebshopTemplate.Models.Basket> Basket { get; set; } = default!;
}
        //#region Relationship Customer - Company
        //modelBuilder.Entity<Customer>()
        //    .HasOne(c => c.RepresentingCompany) // Each Customer has one Company
        //    .WithMany(co => co.Representatives) // Company has many Customers
        //    .HasForeignKey(c => c.CompanyId); // ForeignKey in Customer pointing to Company
        //modelBuilder.Entity<Customer>()
        //    .HasIndex(c => c.CompanyId); // Index on CompanyId in Customer
        //modelBuilder.Entity<Customer>()
        //    .Property(c => c.CompanyId) // CompanyId in Customer
        //    .IsRequired(false); // Is not required
        //modelBuilder.Entity<Customer>()
        //    .Property(c => c.CompanyId) // CompanyId in Customer
        //    .HasDefaultValue(null); // Default value is null
        //modelBuilder.Entity<Customer>()
        //    .Property(c => c.CompanyId) // CompanyId in Customer
        //    .ValueGeneratedNever(); // Value is never generated
        //#endregion