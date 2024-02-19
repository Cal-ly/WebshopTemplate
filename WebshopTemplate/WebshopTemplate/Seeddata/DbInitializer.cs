using WebshopTemplate.Models;
using System.Linq;

namespace WebshopTemplate.Seeddata
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedDatabaseUser()
        {
            ArgumentNullException.ThrowIfNull(_userManager);

            _context.Database.EnsureCreated();

            // Define IdentityUser password and roles
            string password = "1234";
            string managerRole = "Manager";
            string superMemberRole = "SuperMember";
            string memberRole = "Member";

            // Define companies
            var companies = new List<Company>
                {
                    new Company { Name = "Dead'R'Us", Address = "Street 1", City = "City A", PostalCode = "12345", Country = "Denmark", Email = "contact@companya.com", Phone = "12345678", Website = "www.companya.com" },
                    new Company { Name = "FuneralHome", Address = "Street 2", City = "City B", PostalCode = "23456", Country = "Denmark", Email = "contact@companyb.com", Phone = "87654321", Website = "www.companyb.com" }
                };

            // Define customers
            var customers = new List<Customer>();
            var customerUsers = new List<IdentityUser>
                {
                    new IdentityUser { UserName = "john@mail.com", Email = "john@mail.com", EmailConfirmed = true },
                    new IdentityUser { UserName = "jane@mail.com", Email = "jane@mail.com", EmailConfirmed = true },
                    new IdentityUser { UserName = "daddy@mail.com", Email = "daddy@mail.com", EmailConfirmed = true },
                    new IdentityUser { UserName = "mommy@mail.com", Email = "mommy@mail.com" , EmailConfirmed = true},
                    new IdentityUser { UserName = "sonny@mail.com", Email = "sonny@mail.com" , EmailConfirmed = true},
                    new IdentityUser { UserName = "sunny@mail.com", Email = "sunny@mail.com" , EmailConfirmed = true}
                };

            for (int i = 0; i < customerUsers.Count; i++)
            {
                var result = await _userManager.CreateAsync(customerUsers[i], password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(customerUsers[i], i < 5 ? superMemberRole : memberRole);
                }
                customers.Add(new Customer { UserId = customerUsers[i].Id, User = customerUsers[i], FirstName = "FirstName" + i, LastName = "LastName" + i, Address = "Address " + i, City = "City " + i, PostalCode = "PostalCode" + i, Country = "Denmark", Phone = "Phone" + i, RepresentingCompany = i < 4 ? companies[0] : companies[1] });
            }

            await _context.Customers.AddRangeAsync(customers);

            // Define Staff
            var staffMembers = new List<Staff>();
            var staffUsers = new List<IdentityUser>
            {
                new IdentityUser { UserName = "staff1@staff.com", Email = "staff1@staff.com", EmailConfirmed = true },
                new IdentityUser { UserName = "staff2@staff.com", Email = "staff2@staff.com" , EmailConfirmed = true},
                new IdentityUser { UserName = "staff3@staff.com", Email = "staff3@staff.com" , EmailConfirmed = true}
            };

            for (int i = 0; i < staffUsers.Count; i++)
            {
                var result = await _userManager.CreateAsync(staffUsers[i], password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(staffUsers[i], managerRole);
                }
                staffMembers.Add(new Staff { UserId = staffUsers[i].Id, User = staffUsers[i], FirstName = "StaffFirstName" + i, LastName = "StaffLastName" + i, Address = "Address " + (i + 6), City = "City " + (i + 6), PostalCode = "PostalCode" + (i + 6), Country = "Denmark", Phone = "Phone" + (i + 6) });
            }

            await _context.Staffers.AddRangeAsync(staffMembers);

            //Add companies and customers to the database
            await _context.Companies.AddRangeAsync(companies);

            // Add company representatives to the companies
            companies[0].Representatives.Add(customers[0]);
            companies[0].Representatives.Add(customers[1]);
            companies[1].Representatives.Add(customers[2]);
            companies[1].Representatives.Add(customers[3]);
        }
        public async Task SeedDatabaseCategoryAndProducts()
        {
            // Define categories
            var categories = new List<Category>
            {
                new Category { Name = "Fresh Flowers", Description = "Beautiful blooms for every occasion." },
                new Category { Name = "Flower Arrangements", Description = "Expertly crafted bouquets to brighten your space." },
                new Category { Name = "Wedding Flowers", Description = "Elegant flowers for your special day." },
                new Category { Name = "Indoor Plants", Description = "Greenery to enhance your indoor living areas." },
                new Category { Name = "Flower Decorations", Description = "Decorative elements for home and events." }
            };

            // Product Mock Data
            var products = new List<Product>
            {
                // Fresh Flowers
                new Product { Name = "Roses", Image = "roses.jpg", Description = "Red roses symbolizing love and respect.", Price = 29.99m, Quantity = 50, Category = categories[0] },
                new Product { Name = "Tulips", Image = "tulips.jpg", Description = "Colorful tulips for a vibrant touch.", Price = 19.99m, Quantity = 30, Category = categories[0] },
                new Product { Name = "Sunflowers", Image = "sunflowers.jpg", Description = "Bright and cheerful sunflowers.", Price = 24.99m, Quantity = 40, Category = categories[0] },

                // Flower Arrangements
                new Product { Name = "Spring Bouquet", Image = "spring_bouquet.jpg", Description = "A fresh and lively arrangement of spring flowers.", Price = 49.99m, Quantity = 20, Category = categories[1] },
                new Product { Name = "Elegant Orchids", Image = "elegant_orchids.jpg", Description = "Sophisticated orchids for a modern look.", Price = 59.99m, Quantity = 15, Category = categories[1] },
                new Product { Name = "Mixed Bouquet", Image = "mixed_bouquet.jpg", Description = "A mix of colorful flowers for a vibrant display.", Price = 39.99m, Quantity = 25, Category = categories[1] },

                // Wedding Flowers
                new Product { Name = "Bridal Bouquet", Image = "bridal_bouquet.jpg", Description = "A stunning bouquet designed for the beautiful bride.", Price = 99.99m, Quantity = 10, Category = categories[2] },
                new Product { Name = "Wedding Centerpieces", Image = "wedding_centerpieces.jpg", Description = "Elegant centerpieces to adorn your tables.", Price = 120.99m, Quantity = 5, Category = categories[2] },
                new Product { Name = "Boutonnieres", Image = "boutonnieres.jpg", Description = "Charming boutonnieres for the groom and groomsmen.", Price = 14.99m, Quantity = 20, Category = categories[2] },

                // Indoor Plants
                new Product { Name = "Fiddle Leaf Fig", Image = "fiddle_leaf_fig.jpg", Description = "A trendy and robust indoor plant.", Price = 89.99m, Quantity = 10, Category = categories[3] },
                new Product { Name = "Snake Plant", Image = "snake_plant.jpg", Description = "An easy-to-care-for plant for indoor air purification.", Price = 39.99m, Quantity = 20, Category = categories[3] },
                new Product { Name = "Pothos", Image = "pothos.jpg", Description = "A versatile and low-maintenance indoor plant.", Price = 29.99m, Quantity = 30, Category = categories[3] },

                // Flower Decorations
                new Product { Name = "Floral Candle Rings", Image = "floral_candle_rings.jpg", Description = "Add a floral touch to your candlelit evenings.", Price = 14.99m, Quantity = 40, Category = categories[4] },
                new Product { Name = "Flower Wall Decor", Image = "flower_wall_decor.jpg", Description = "Beautiful wall decor to enhance any room.", Price = 75.99m, Quantity = 15, Category = categories[4] },
                new Product { Name = "Floral Wreath", Image = "floral_wreath.jpg", Description = "A charming wreath to adorn your door.", Price = 49.99m, Quantity = 20, Category = categories[4] },
            };

            // Linking Products to Categories
            foreach (var category in categories)
            {
                category.Products = products.Where(p => p.Category == category).ToList();
            }

            await _context.Categories.AddRangeAsync(categories);
        }
        public async Task SeedDatabaseOrders()
        {
            // Define customers
            var customers = await _context.Customers.ToListAsync();
            if (customers.Count == 0 || customers.Count < 6)
            {
                throw new ArgumentException("Customers - List is empty or has less than 6 members");
            }

            // Define Staff
            var staffMembers = await _context.Staffers.ToListAsync();
            if (staffMembers.Count == 0 || staffMembers.Count < 3)
            {
                throw new ArgumentException("StaffMembers - List is empty or has less than 3 members");
            }

            var products = await _context.Products.ToListAsync();
            if (products.Count == 0 || products.Count < 16)
            {
                throw new ArgumentException("Products - List is empty or has less than 16 members");
            }

            // Define Orders
            var orders = new List<Order>
            {
                new Order { CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-0), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 1", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-1), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 2", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[2].Id!, Customer = customers[2], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-2), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 3", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[3].Id!, Customer = customers[3], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-3), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 4", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[4].Id!, Customer = customers[4], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-4), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 5", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[5].Id!, Customer = customers[5], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-5), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 6", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-6), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 7", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-7), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 8", CommentFromShop = "Confirmed, processing your order." },
            };

            // Define OrderDetails
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId = orders[0].Id, Order = orders[0], ProductId = products[0].Id, ProductInOrder = products[0], Quantity = 2, Price = products[0].Price },
                new OrderDetail { OrderId = orders[0].Id, Order = orders[0], ProductId = products[1].Id, ProductInOrder = products[1], Quantity = 3, Price = products[1].Price },
                new OrderDetail { OrderId = orders[1].Id, Order = orders[1], ProductId = products[2].Id, ProductInOrder = products[2], Quantity = 1, Price = products[2].Price },
                new OrderDetail { OrderId = orders[1].Id, Order = orders[1], ProductId = products[3].Id, ProductInOrder = products[3], Quantity = 4, Price = products[3].Price },
                new OrderDetail { OrderId = orders[2].Id, Order = orders[2], ProductId = products[4].Id, ProductInOrder = products[4], Quantity = 2, Price = products[4].Price },
                new OrderDetail { OrderId = orders[2].Id, Order = orders[2], ProductId = products[5].Id, ProductInOrder = products[5], Quantity = 3, Price = products[5].Price },
                new OrderDetail { OrderId = orders[3].Id, Order = orders[3], ProductId = products[6].Id, ProductInOrder = products[6], Quantity = 1, Price = products[6].Price },
                new OrderDetail { OrderId = orders[3].Id, Order = orders[3], ProductId = products[7].Id, ProductInOrder = products[7], Quantity = 4, Price = products[7].Price },
                new OrderDetail { OrderId = orders[4].Id, Order = orders[4], ProductId = products[8].Id, ProductInOrder = products[8], Quantity = 2, Price = products[8].Price },
                new OrderDetail { OrderId = orders[4].Id, Order = orders[4], ProductId = products[9].Id, ProductInOrder = products[9], Quantity = 3, Price = products[9].Price },
                new OrderDetail { OrderId = orders[5].Id, Order = orders[5], ProductId = products[10].Id, ProductInOrder = products[10], Quantity = 1, Price = products[10].Price },
                new OrderDetail { OrderId = orders[5].Id, Order = orders[5], ProductId = products[11].Id, ProductInOrder = products[11], Quantity = 4, Price = products[11].Price },
                new OrderDetail { OrderId = orders[6].Id, Order = orders[6], ProductId = products[12].Id, ProductInOrder = products[12], Quantity = 2, Price = products[12].Price },
                new OrderDetail { OrderId = orders[6].Id, Order = orders[6], ProductId = products[13].Id, ProductInOrder = products[13], Quantity = 3, Price = products[13].Price },
                new OrderDetail { OrderId = orders[7].Id, Order = orders[7], ProductId = products[14].Id, ProductInOrder = products[14], Quantity = 1, Price = products[14].Price },
                new OrderDetail { OrderId = orders[7].Id, Order = orders[7], ProductId = products[15].Id, ProductInOrder = products[15], Quantity = 4, Price = products[15].Price },
            };

            // Automatically associate OrderDetails with Orders
            foreach (var order in orders)
            {
                order.OrderDetails = orderDetails.Where(od => od.Order == order).ToList();
            }

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
        }
        public async Task SeedDatabaseStaged()
        {
            await SeedDatabaseUser();
            await SeedDatabaseCategoryAndProducts();
            await SeedDatabaseOrders();

            await _context.SaveChangesAsync();
        }
        public async Task GodSeedDatabase()
        {
            ArgumentNullException.ThrowIfNull(_userManager);

            _context.Database.EnsureCreated();

            // Define IdentityUser password and roles
            string password = "1234";
            string managerRole = "Manager";
            string superMemberRole = "SuperMember";
            string memberRole = "Member";

            // Define companies
            var companies = new List<Company>
            {
                new Company { Name = "Dead'R'Us", Address = "Street 1", City = "City A", PostalCode = "12345", Country = "Denmark", Email = "contact@companya.com", Phone = "12345678", Website = "www.companya.com" },
                new Company { Name = "FuneralHome", Address = "Street 2", City = "City B", PostalCode = "23456", Country = "Denmark", Email = "contact@companyb.com", Phone = "87654321", Website = "www.companyb.com" }
            };

            // Define customers
            var customers = new List<Customer>();
            var customerUsers = new List<IdentityUser>
            {
                new IdentityUser { UserName = "john@mail.com", Email = "john@mail.com", EmailConfirmed = true },
                new IdentityUser { UserName = "jane@mail.com", Email = "jane@mail.com", EmailConfirmed = true },
                new IdentityUser { UserName = "daddy@mail.com", Email = "daddy@mail.com", EmailConfirmed = true },
                new IdentityUser { UserName = "mommy@mail.com", Email = "mommy@mail.com" , EmailConfirmed = true},
                new IdentityUser { UserName = "sonny@mail.com", Email = "sonny@mail.com" , EmailConfirmed = true},
                new IdentityUser { UserName = "sunny@mail.com", Email = "sunny@mail.com" , EmailConfirmed = true}
            };

            for (int i = 0; i < customerUsers.Count; i++)
            {
                var result = await _userManager.CreateAsync(customerUsers[i], password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(customerUsers[i], i < 5 ? superMemberRole : memberRole);
                }
                customers.Add(new Customer { UserId = customerUsers[i].Id, User = customerUsers[i], FirstName = "FirstName" + i, LastName = "LastName" + i, Address = "Address " + i, City = "City " + i, PostalCode = "PostalCode" + i, Country = "Denmark", Phone = "Phone" + i, RepresentingCompany = i < 4 ? companies[0] : companies[1] });
            }

            await _context.Customers.AddRangeAsync(customers);

            // Define Staff
            var staffMembers = new List<Staff>();
            var staffUsers = new List<IdentityUser>
            {
                new IdentityUser { UserName = "staff1@staff.com", Email = "staff1@staff.com", EmailConfirmed = true },
                new IdentityUser { UserName = "staff2@staff.com", Email = "staff2@staff.com" , EmailConfirmed = true},
                new IdentityUser { UserName = "staff3@staff.com", Email = "staff3@staff.com" , EmailConfirmed = true}
            };

            for (int i = 0; i < staffUsers.Count; i++)
            {
                var result = await _userManager.CreateAsync(staffUsers[i], password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(staffUsers[i], managerRole);
                }
                staffMembers.Add(new Staff { UserId = staffUsers[i].Id, User = staffUsers[i], FirstName = "StaffFirstName" + i, LastName = "StaffLastName" + i, Address = "Address " + (i + 6), City = "City " + (i + 6), PostalCode = "PostalCode" + (i + 6), Country = "Denmark", Phone = "Phone" + (i + 6) });
            }

            await _context.Staffers.AddRangeAsync(staffMembers);

            //Add companies and customers to the database
            await _context.Companies.AddRangeAsync(companies);

            // Add company representatives to the companies
            companies[0].Representatives.Add(customers[0]);
            companies[0].Representatives.Add(customers[1]);
            companies[1].Representatives.Add(customers[2]);
            companies[1].Representatives.Add(customers[3]);

            // Define categories
            var categories = new List<Category>
            {
                new Category { Name = "Fresh Flowers", Description = "Beautiful blooms for every occasion." },
                new Category { Name = "Flower Arrangements", Description = "Expertly crafted bouquets to brighten your space." },
                new Category { Name = "Wedding Flowers", Description = "Elegant flowers for your special day." },
                new Category { Name = "Indoor Plants", Description = "Greenery to enhance your indoor living areas." },
                new Category { Name = "Flower Decorations", Description = "Decorative elements for home and events." }
            };

            // Product Mock Data
            var products = new List<Product>
            {
                // Fresh Flowers
                new Product { Name = "Roses", Image = "roses.jpg", Description = "Red roses symbolizing love and respect.", Price = 29.99m, Quantity = 50, Category = categories[0] },
                new Product { Name = "Tulips", Image = "tulips.jpg", Description = "Colorful tulips for a vibrant touch.", Price = 19.99m, Quantity = 30, Category = categories[0] },
                new Product { Name = "Sunflowers", Image = "sunflowers.jpg", Description = "Bright and cheerful sunflowers.", Price = 24.99m, Quantity = 40, Category = categories[0] },

                // Flower Arrangements
                new Product { Name = "Spring Bouquet", Image = "spring_bouquet.jpg", Description = "A fresh and lively arrangement of spring flowers.", Price = 49.99m, Quantity = 20, Category = categories[1] },
                new Product { Name = "Elegant Orchids", Image = "elegant_orchids.jpg", Description = "Sophisticated orchids for a modern look.", Price = 59.99m, Quantity = 15, Category = categories[1] },
                new Product { Name = "Mixed Bouquet", Image = "mixed_bouquet.jpg", Description = "A mix of colorful flowers for a vibrant display.", Price = 39.99m, Quantity = 25, Category = categories[1] },

                // Wedding Flowers
                new Product { Name = "Bridal Bouquet", Image = "bridal_bouquet.jpg", Description = "A stunning bouquet designed for the beautiful bride.", Price = 99.99m, Quantity = 10, Category = categories[2] },
                new Product { Name = "Wedding Centerpieces", Image = "wedding_centerpieces.jpg", Description = "Elegant centerpieces to adorn your tables.", Price = 120.99m, Quantity = 5, Category = categories[2] },
                new Product { Name = "Boutonnieres", Image = "boutonnieres.jpg", Description = "Charming boutonnieres for the groom and groomsmen.", Price = 14.99m, Quantity = 20, Category = categories[2] },

                // Indoor Plants
                new Product { Name = "Fiddle Leaf Fig", Image = "fiddle_leaf_fig.jpg", Description = "A trendy and robust indoor plant.", Price = 89.99m, Quantity = 10, Category = categories[3] },
                new Product { Name = "Snake Plant", Image = "snake_plant.jpg", Description = "An easy-to-care-for plant for indoor air purification.", Price = 39.99m, Quantity = 20, Category = categories[3] },
                new Product { Name = "Pothos", Image = "pothos.jpg", Description = "A versatile and low-maintenance indoor plant.", Price = 29.99m, Quantity = 30, Category = categories[3] },

                // Flower Decorations
                new Product { Name = "Floral Candle Rings", Image = "floral_candle_rings.jpg", Description = "Add a floral touch to your candlelit evenings.", Price = 14.99m, Quantity = 40, Category = categories[4] },
                new Product { Name = "Flower Wall Decor", Image = "flower_wall_decor.jpg", Description = "Beautiful wall decor to enhance any room.", Price = 75.99m, Quantity = 15, Category = categories[4] },
                new Product { Name = "Floral Wreath", Image = "floral_wreath.jpg", Description = "A charming wreath to adorn your door.", Price = 49.99m, Quantity = 20, Category = categories[4] },
            };

            // Linking Products to Categories
            foreach (var category in categories)
            {
                category.Products = products.Where(p => p.Category == category).ToList();
            }

            await _context.Categories.AddRangeAsync(categories);

            foreach (var customer in customers.Where(customer => customer.Id == null))
            {
                throw new ArgumentNullException($"{customer.FullName} - Customer Id is null");
            }

            await _context.SaveChangesAsync();

            // Define Orders
            var orders = new List<Order>
            {
                new Order { CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-0), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 1", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-1), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 2", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[2].Id!, Customer = customers[2], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-2), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 3", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[3].Id!, Customer = customers[3], StaffId = staffMembers[1].Id, Staff = staffMembers[1], OrderDate = DateTime.Now.AddDays(-3), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 4", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[4].Id!, Customer = customers[4], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-4), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 5", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[5].Id!, Customer = customers[5], StaffId = staffMembers[2].Id, Staff = staffMembers[2], OrderDate = DateTime.Now.AddDays(-5), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 6", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[0].Id!, Customer = customers[0], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-6), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 7", CommentFromShop = "Confirmed, processing your order." },
                new Order { CustomerId = customers[1].Id!, Customer = customers[1], StaffId = staffMembers[0].Id, Staff = staffMembers[0], OrderDate = DateTime.Now.AddDays(-7), Status = OrderStatus.Submitted, CommentFromUser = "Special instructions for order 8", CommentFromShop = "Confirmed, processing your order." },
            };

            // Define OrderDetails
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId = orders[0].Id, Order = orders[0], ProductId = products[0].Id, ProductInOrder = products[0], Quantity = 2, Price = products[0].Price },
                new OrderDetail { OrderId = orders[0].Id, Order = orders[0], ProductId = products[1].Id, ProductInOrder = products[1], Quantity = 3, Price = products[1].Price },
                new OrderDetail { OrderId = orders[1].Id, Order = orders[1], ProductId = products[2].Id, ProductInOrder = products[2], Quantity = 1, Price = products[2].Price },
                new OrderDetail { OrderId = orders[1].Id, Order = orders[1], ProductId = products[3].Id, ProductInOrder = products[3], Quantity = 4, Price = products[3].Price },
                new OrderDetail { OrderId = orders[2].Id, Order = orders[2], ProductId = products[4].Id, ProductInOrder = products[4], Quantity = 2, Price = products[4].Price },
                new OrderDetail { OrderId = orders[2].Id, Order = orders[2], ProductId = products[5].Id, ProductInOrder = products[5], Quantity = 3, Price = products[5].Price },
                new OrderDetail { OrderId = orders[3].Id, Order = orders[3], ProductId = products[6].Id, ProductInOrder = products[6], Quantity = 1, Price = products[6].Price },
                new OrderDetail { OrderId = orders[3].Id, Order = orders[3], ProductId = products[7].Id, ProductInOrder = products[7], Quantity = 4, Price = products[7].Price },
                new OrderDetail { OrderId = orders[4].Id, Order = orders[4], ProductId = products[8].Id, ProductInOrder = products[8], Quantity = 2, Price = products[8].Price },
                new OrderDetail { OrderId = orders[4].Id, Order = orders[4], ProductId = products[9].Id, ProductInOrder = products[9], Quantity = 3, Price = products[9].Price },
                new OrderDetail { OrderId = orders[5].Id, Order = orders[5], ProductId = products[10].Id, ProductInOrder = products[10], Quantity = 1, Price = products[10].Price },
                new OrderDetail { OrderId = orders[5].Id, Order = orders[5], ProductId = products[11].Id, ProductInOrder = products[11], Quantity = 4, Price = products[11].Price },
                new OrderDetail { OrderId = orders[6].Id, Order = orders[6], ProductId = products[12].Id, ProductInOrder = products[12], Quantity = 2, Price = products[12].Price },
                new OrderDetail { OrderId = orders[6].Id, Order = orders[6], ProductId = products[13].Id, ProductInOrder = products[13], Quantity = 3, Price = products[13].Price },
                new OrderDetail { OrderId = orders[7].Id, Order = orders[7], ProductId = products[14].Id, ProductInOrder = products[14], Quantity = 1, Price = products[14].Price },
                new OrderDetail { OrderId = orders[7].Id, Order = orders[7], ProductId = products[15].Id, ProductInOrder = products[15], Quantity = 4, Price = products[15].Price },
            };

            // Automatically associate OrderDetails with Orders
            foreach (var order in orders)
            {
                order.OrderDetails = orderDetails.Where(od => od.Order == order).ToList();
            }

            await _context.Orders.AddRangeAsync(orders);

            await _context.SaveChangesAsync();
        }
    }
}