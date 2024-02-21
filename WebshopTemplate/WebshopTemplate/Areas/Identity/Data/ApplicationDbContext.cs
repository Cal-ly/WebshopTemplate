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
    public DbSet<IdentityUser> IdentityUsers { get; set; }
    public DbSet<Staff> Staffers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Basket> Baskets { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Relationship IdentityUser
        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("IdentityUsers");
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
            entity.Property(c => c.CustomerCreated).HasDefaultValueSql("GETDATE()"); // Default value for CustomerCreated
            entity.HasIndex(c => c.Id);
            entity.HasOne(c => c.User)
                    .WithOne()
                        .HasForeignKey<Customer>(c => c.UserId); // One-to-One relationship between Customer and IdentityUser

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
            entity.ToTable("Staffers");
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.Property(s => s.EmploymentDate).HasDefaultValueSql("GETDATE()"); // Default value for EmploymentDate
            entity.HasIndex(s => s.Id);
            entity.HasOne(s => s.User).WithOne().HasForeignKey<Staff>(s => s.UserId); // One-to-One relationship between Staff and IdentityUser

            entity.HasMany(s => s.Orders) // Staff has many Orders
                    .WithOne(o => o.Staff) // Each Order has one Staff
                        .HasForeignKey(o => o.StaffId); // ForeignKey in Order pointing to Staff
        });
        #endregion

        #region Relationship Company
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.Property(co => co.Id).ValueGeneratedOnAdd();
            entity.HasIndex(co => co.Id);
            entity.HasMany(co => co.Representatives) // Company has many Customers
                    .WithOne(c => c.RepresentingCompany) // Each Customer has one Company
                        .HasForeignKey(c => c.CompanyId); // ForeignKey in Customer pointing to Company
        });
        #endregion

        #region Relationship Order - OrderDetail - Product
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.Property(o => o.Id).ValueGeneratedOnAdd();
            entity.HasIndex(o => o.Id); // Index on Id in Order
            entity.HasMany(o => o.OrderDetails) // Order has many OrderDetails
                    .WithOne(od => od.Order) // Each OrderDetail has one Order
                        .HasForeignKey(od => od.OrderId); // ForeignKey in OrderDetail pointing to Order
        });
        #endregion

        #region Relationship OrderDetail - Product
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetails");
            entity.Property(od => od.Id).ValueGeneratedOnAdd();
            entity.HasIndex(od => od.Id);
            entity.HasOne(od => od.ProductInOrder) // Each OrderDetail has one Product
                    .WithMany(p => p.OrderDetails) // Product has many OrderDetails
                        .HasForeignKey(od => od.ProductId); // ForeignKey in OrderDetail pointing to Product
        });
        #endregion

        #region Relationship Product - Category
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.HasIndex(p => p.CategoryId); // Index on CategoryId in Product
            entity.HasOne(p => p.Category) // Each Product has one Category
                    .WithMany(c => c.Products) // Category has many Products
                        .HasForeignKey(p => p.CategoryId); // ForeignKey in Product pointing to Category
            //entity.HasMany(p => p.OrderDetails) // Product has many OrderDetails
            //        .WithOne(od => od.ProductInOrder) // Each OrderDetail has one Product
            //            .HasForeignKey(od => od.ProductId); // ForeignKey in OrderDetail pointing to Product
        });
        #endregion

        #region Relationship Category - Product
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.HasIndex(c => c.Id); // Index on Id in Category
            entity.HasMany(c => c.Products) // Category has many Products
                    .WithOne(p => p.Category) // Each Product has one Category
                        .HasForeignKey(p => p.CategoryId); // ForeignKey in Product pointing to Category
        });
        #endregion

        #region Relationship Basket - BasketItem - Product
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.ToTable("Baskets");
            entity.Property(b => b.Id).ValueGeneratedOnAdd();
            entity.HasIndex(b => b.Id); // Index on Id in Basket
            entity.HasOne(b => b.Customer)
                  .WithOne()
                  .HasForeignKey<Basket>(b => b.CustomerId); // One-to-One relationship between Basket and Customer
            entity.HasMany(b => b.Items) // Basket has many BasketItems
                    .WithOne(bi => bi.Basket) // Each BasketItem has one Basket
                        .HasForeignKey(bi => bi.BasketId); // ForeignKey in BasketItem pointing to Basket
        });
        #endregion

        #region Relationship BasketItem - Product
        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.ToTable("BasketItems");
            entity.Property(bi => bi.Id).ValueGeneratedOnAdd();
            entity.HasIndex(bi => bi.Id); // Index on Id in BasketItem
            entity.HasOne(bi => bi.ProductInBasket).WithOne().HasForeignKey<BasketItem>(bi => bi.ProductId); // Each BasketItem has one Product
        });
        #endregion

    }
}