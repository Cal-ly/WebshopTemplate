namespace WebshopTemplate.Initializers;

public class Initializer
{
    // Define IdentityUser password and roles
    private string password = "1234";
    private string managerRole = "Manager";
    private string superMemberRole = "SuperMember";
    private string memberRole = "Member";
    private string adminRole = "Admin";
    private string adminEmail = "admin@admin.com";
    private string adminPassword = "1234";
    private List<Company> companies = new List<Company>();
    private List<Customer> customers = new List<Customer>();
    private List<IdentityUser> customerUsers = new List<IdentityUser>();
    private List<IdentityUser> staffUsers = new List<IdentityUser>();
    private List<Staff> staffMembers = new List<Staff>();
    private List<Order> orders = new List<Order>();
    private List<OrderDetail> orderDetails = new List<OrderDetail>();
    private List<Category> categories = new List<Category>();
    private List<Product> products = new List<Product>();

    public async Task Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        await SeedRoles(serviceProvider);
        await SeedAdmin(serviceProvider);
        await SeedDatabaseUser(serviceProvider, context);
        await SeedDatabaseMockdata(context);
        await context.SaveChangesAsync();
    }
    public async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Manager", "SuperMember", "Member" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    public async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var admin = await userManager.FindByEmailAsync(adminEmail.Normalize());

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            admin = new IdentityUser();
            admin.UserName = adminEmail;
            admin.Email = adminEmail;
            admin.EmailConfirmed = true;
            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, adminRole);
        }
    }
    public async Task SeedDatabaseUser(IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        ArgumentNullException.ThrowIfNull(userManager);

        // Define companies
        companies.AddRange(new List<Company>
        {
            new Company { Name = "Dead'R'Us", Address = "Street 1", City = "City A", PostalCode = "12345", Country = "Denmark", Email = "contact@companya.com", Phone = "12345678", Website = "www.companya.com" },
            new Company { Name = "FuneralHome", Address = "Street 2", City = "City B", PostalCode = "23456", Country = "Denmark", Email = "contact@companyb.com", Phone = "87654321", Website = "www.companyb.com" }
        });

        // Define customers
        customerUsers.AddRange(new List<IdentityUser>
        {
            new IdentityUser { UserName = "john@mail.com", Email = "john@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "jane@mail.com", Email = "jane@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "daddy@mail.com", Email = "daddy@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "mommy@mail.com", Email = "mommy@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "sonny@mail.com", Email = "sonny@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "sunny@mail.com", Email = "sunny@mail.com", EmailConfirmed = true }
        });

        for (int i = 0; i < customerUsers.Count; i++)
        {
            var result = await userManager.CreateAsync(customerUsers[i], password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customerUsers[i], i < 5 ? superMemberRole : memberRole);
            }
            customers.Add(new Customer { Id = Guid.NewGuid().ToString(), UserId = customerUsers[i].Id, User = customerUsers[i], FirstName = "FirstName" + i, LastName = "LastName" + i, Address = "Address " + i, City = "City " + i, PostalCode = "PostalCode" + i, Country = "Denmark", Phone = "Phone" + i, RepresentingCompany = i < 4 ? companies[0] : companies[1] });
        }

        // Add company representatives to the companies
        companies[0].Representatives.Add(customers[0]);
        companies[0].Representatives.Add(customers[1]);
        companies[1].Representatives.Add(customers[2]);
        companies[1].Representatives.Add(customers[3]);

        //Add companies and customers to the database
        await context.Customers.AddRangeAsync(customers);
        await context.Companies.AddRangeAsync(companies);

        // Define Staff
        staffUsers.AddRange(new List<IdentityUser>
        {
            new IdentityUser { UserName = "staff1@staff.com", Email = "staff1@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "staff2@staff.com", Email = "staff2@mail.com", EmailConfirmed = true },
            new IdentityUser { UserName = "staff3@staff.com", Email = "staff3@mail.com", EmailConfirmed = true }
        });

        for (int i = 0; i < staffUsers.Count; i++)
        {
            var result = await userManager.CreateAsync(staffUsers[i], password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(staffUsers[i], managerRole);
            }
            int j = i + 6;
            staffMembers.Add(new Staff { Id = Guid.NewGuid().ToString(), UserId = staffUsers[i].Id, User = staffUsers[i], FirstName = "StaffFirstName" + i, LastName = "StaffLastName" + i, Address = "Address " + j, City = "City " + j, PostalCode = "PostalCode" + j, Country = "Denmark", Phone = "Phone" + j });
        }

        await context.Staffers.AddRangeAsync(staffMembers);
    }
    public async Task SeedDatabaseMockdata(ApplicationDbContext context)
    {
        categories.AddRange(new List<Category>
        {
                new Category { Id = Guid.NewGuid().ToString(), Name = "Fresh Flowers", Description = "Beautiful blooms for every occasion." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Flower Arrangements", Description = "Expertly crafted bouquets to brighten your space." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Wedding Flowers", Description = "Elegant flowers for your special day." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Indoor Plants", Description = "Greenery to enhance your indoor living areas." },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Flower Decorations", Description = "Decorative elements for home and events." }
        });
        await context.Categories.AddRangeAsync(categories);

        products.AddRange(new List<Product>
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
        });

        await context.Products.AddRangeAsync(products);

        // Define Orders
        orders.AddRange(new List<Order>
        {
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-0), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 1", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-1), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 2", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[2].Id!, Customer = customers[2], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-2), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 3", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[3].Id!, Customer = customers[3], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-3), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 4", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[4].Id!, Customer = customers[4], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-4), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 5", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[5].Id!, Customer = customers[5], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-5), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 6", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-6), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 7", CommentFromShop = "Confirmed, processing your order." },
            new Order { Id = Guid.NewGuid().ToString(), CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-7), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 8", CommentFromShop = "Confirmed, processing your order." },
        });

        // Define OrderDetails
        orderDetails.AddRange(new List<OrderDetail>
        {
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[0].Id, Order = orders[0], ProductId = products[0].Id, ProductInOrder = products[0], Quantity = 2, Price = products[0].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[0].Id, Order = orders[0], ProductId = products[1].Id, ProductInOrder = products[1], Quantity = 3, Price = products[1].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[1].Id, Order = orders[1], ProductId = products[2].Id, ProductInOrder = products[2], Quantity = 1, Price = products[2].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[1].Id, Order = orders[1], ProductId = products[3].Id, ProductInOrder = products[3], Quantity = 4, Price = products[3].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[2].Id, Order = orders[2], ProductId = products[4].Id, ProductInOrder = products[4], Quantity = 2, Price = products[4].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[2].Id, Order = orders[2], ProductId = products[5].Id, ProductInOrder = products[5], Quantity = 3, Price = products[5].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[3].Id, Order = orders[3], ProductId = products[6].Id, ProductInOrder = products[6], Quantity = 1, Price = products[6].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[3].Id, Order = orders[3], ProductId = products[7].Id, ProductInOrder = products[7], Quantity = 4, Price = products[7].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[4].Id, Order = orders[4], ProductId = products[8].Id, ProductInOrder = products[8], Quantity = 2, Price = products[8].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[4].Id, Order = orders[4], ProductId = products[9].Id, ProductInOrder = products[9], Quantity = 3, Price = products[9].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[5].Id, Order = orders[5], ProductId = products[10].Id, ProductInOrder = products[10], Quantity = 1, Price = products[10].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[5].Id, Order = orders[5], ProductId = products[11].Id, ProductInOrder = products[11], Quantity = 4, Price = products[11].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[6].Id, Order = orders[6], ProductId = products[12].Id, ProductInOrder = products[12], Quantity = 2, Price = products[12].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[6].Id, Order = orders[6], ProductId = products[13].Id, ProductInOrder = products[13], Quantity = 3, Price = products[13].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[7].Id, Order = orders[7], ProductId = products[14].Id, ProductInOrder = products[14], Quantity = 1, Price = products[14].Price },
            new OrderDetail { Id = Guid.NewGuid().ToString(), OrderId = orders[7].Id, Order = orders[7], ProductId = products[0].Id, ProductInOrder = products[0], Quantity = 4, Price = products[0].Price },
        });

        // Automatically associate OrderDetails with Orders
        foreach (var order in orders)
        {
            order.OrderDetails = orderDetails.Where(od => od.Order == order).ToList();
        }
        await context.Orders.AddRangeAsync(orders);

    }
}