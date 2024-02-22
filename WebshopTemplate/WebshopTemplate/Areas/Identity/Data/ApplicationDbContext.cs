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

        #region Object Initialization

        var categories = new List<Category>
        {
                new Category { Id = Guid.NewGuid().ToString(), Name = "Fresh Flowers", Description = "Beautiful blooms for every occasion." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Flower Arrangements", Description = "Expertly crafted bouquets to brighten your space." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Wedding Flowers", Description = "Elegant flowers for your special day." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Indoor Plants", Description = "Greenery to enhance your indoor living areas." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Flower Decorations", Description = "Decorative elements for home and events." }
        };

        var products = new List<Product>
        {
            // Fresh Flowers
            new Product { Id = Guid.NewGuid().ToString(), Name = "Roses", Image = "roses.jpg", Description = "Red roses symbolizing love and respect.", Price = 29.99m, Quantity = 50, CategoryId = categories[0].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Tulips", Image = "tulips.jpg", Description = "Colorful tulips for a vibrant touch.", Price = 19.99m, Quantity = 30, CategoryId = categories[0].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Sunflowers", Image = "sunflowers.jpg", Description = "Bright and cheerful sunflowers.", Price = 24.99m, Quantity = 40, CategoryId = categories[0].Id },

            // Flower Arrangements
            new Product { Id = Guid.NewGuid().ToString(), Name = "Spring Bouquet", Image = "spring_bouquet.jpg", Description = "A fresh and lively arrangement of spring flowers.", Price = 49.99m, Quantity = 20, CategoryId = categories[1].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Elegant Orchids", Image = "elegant_orchids.jpg", Description = "Sophisticated orchids for a modern look.", Price = 59.99m, Quantity = 15, CategoryId = categories[1].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Mixed Bouquet", Image = "mixed_bouquet.jpg", Description = "A mix of colorful flowers for a vibrant display.", Price = 39.99m, Quantity = 25, CategoryId = categories[1].Id },

            // Wedding Flowers
            new Product { Id = Guid.NewGuid().ToString(), Name = "Bridal Bouquet", Image = "bridal_bouquet.jpg", Description = "A stunning bouquet designed for the beautiful bride.", Price = 99.99m, Quantity = 10, CategoryId = categories[2].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Wedding Centerpieces", Image = "wedding_centerpieces.jpg", Description = "Elegant centerpieces to adorn your tables.", Price = 120.99m, Quantity = 5, CategoryId = categories[2].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Boutonnieres", Image = "boutonnieres.jpg", Description = "Charming boutonnieres for the groom and groomsmen.", Price = 14.99m, Quantity = 20, CategoryId = categories[2].Id },

            // Indoor Plants
            new Product { Id = Guid.NewGuid().ToString(), Name = "Fiddle Leaf Fig", Image = "fiddle_leaf_fig.jpg", Description = "A trendy and robust indoor plant.", Price = 89.99m, Quantity = 10, CategoryId = categories[3].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Snake Plant", Image = "snake_plant.jpg", Description = "An easy-to-care-for plant for indoor air purification.", Price = 39.99m, Quantity = 20, CategoryId = categories[3].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Pothos", Image = "pothos.jpg", Description = "A versatile and low-maintenance indoor plant.", Price = 29.99m, Quantity = 30, CategoryId = categories[3].Id },

            // Flower Decorations
            new Product { Id = Guid.NewGuid().ToString(), Name = "Floral Candle Rings", Image = "floral_candle_rings.jpg", Description = "Add a floral touch to your candlelit evenings.", Price = 14.99m, Quantity = 40, CategoryId = categories[4].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Flower Wall Decor", Image = "flower_wall_decor.jpg", Description = "Beautiful wall decor to enhance any room.", Price = 75.99m, Quantity = 15, CategoryId = categories[4].Id },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Floral Wreath", Image = "floral_wreath.jpg", Description = "A charming wreath to adorn your door.", Price = 49.99m, Quantity = 20, CategoryId = categories[4].Id },
        };

        #endregion

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
                        .HasForeignKey(c => c.CompanyId)
                            .IsRequired(false); // Make the relationship optional (nullable)
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
            entity.HasOne(s => s.User)
                    .WithOne()
                        .HasForeignKey<Staff>(s => s.UserId); // One-to-One relationship between Staff and IdentityUser

            entity.HasMany(s => s.Orders) // Staff has many Orders
                    .WithOne(o => o.Staff) // Each Order has one Staff
                        .HasForeignKey(o => o.StaffId) // ForeignKey in Order pointing to Staff
                            .IsRequired(false); // Make the relationship optional (nullable)
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
                        .HasForeignKey(c => c.CompanyId) // ForeignKey in Customer pointing to Company
                            .IsRequired(false); // Make the relationship optional (nullable)
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
                        .HasForeignKey(od => od.OrderId) // ForeignKey in OrderDetail pointing to Order
                            .IsRequired(false); // Make the relationship optional (nullable)
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
                        .HasForeignKey(od => od.ProductId) // ForeignKey in OrderDetail pointing to Product
                            .IsRequired(false); // Make the relationship optional (nullable)
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
                        .HasForeignKey(p => p.CategoryId) // ForeignKey in Product pointing to Category
                            .IsRequired(false); // Make the relationship optional (nullable)
            entity.HasData(categories);
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
                        .HasForeignKey(p => p.CategoryId) // ForeignKey in Product pointing to Category
                            .IsRequired(false); // Make the relationship optional (nullable)
            entity.HasMany(p => p.OrderDetails) // Product has many OrderDetails
                    .WithOne(od => od.ProductInOrder) // Each OrderDetail has one Product
                        .HasForeignKey(od => od.ProductId) // ForeignKey in OrderDetail pointing to Product
                            .IsRequired(false); // Make the relationship optional (nullable)
            entity.HasData(products);
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
                        .HasForeignKey(bi => bi.BasketId) // ForeignKey in BasketItem pointing to Basket
                            .IsRequired(false); // Make the relationship optional (nullable)
        });
        #endregion

        #region Relationship BasketItem - Product
        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.ToTable("BasketItems");
            entity.Property(bi => bi.Id).ValueGeneratedOnAdd();
            entity.HasIndex(bi => bi.Id); // Index on Id in BasketItem
            entity.HasOne(bi => bi.ProductInBasket)
                    .WithOne()
                        .HasForeignKey<BasketItem>(bi => bi.ProductId); // Each BasketItem has one Product
        });
        #endregion

    }
}