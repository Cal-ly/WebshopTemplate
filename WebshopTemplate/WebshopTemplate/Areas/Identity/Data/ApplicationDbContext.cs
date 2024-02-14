using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Models;

namespace WebshopTemplate.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Relationship Customer - Company
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.RepresentingCompany) // Each Customer has one Company
            .WithMany(co => co.Representatives) // Company has many Customers
            .HasForeignKey(c => c.CompanyId); // ForeignKey in Customer pointing to Company
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.CompanyId); // Index on CompanyId in Customer
        modelBuilder.Entity<Customer>()
            .Property(c => c.CompanyId) // CompanyId in Customer
            .IsRequired(false); // Is not required
        modelBuilder.Entity<Customer>()
            .Property(c => c.CompanyId) // CompanyId in Customer
            .HasDefaultValue(null); // Default value is null
        modelBuilder.Entity<Customer>()
            .Property(c => c.CompanyId) // CompanyId in Customer
            .ValueGeneratedNever(); // Value is never generated

        #endregion

        #region Relationship Order - Customer
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders) // Customer has many Orders
            .WithOne(o => o.Customer) // Each Order has one Customer
            .HasForeignKey(o => o.UserId); // ForeignKey in Order pointing to Customer
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Id); // Index on Id in Customer
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
}