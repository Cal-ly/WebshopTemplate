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

        public async Task SeedDatabase()
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
                    new IdentityUser { UserName = "JohnDead", Email = "john@mail.com" },
                    new IdentityUser { UserName = "JaneDead", Email = "jane@mail.com" },
                    new IdentityUser { UserName = "EpicDaddy", Email = "daddy@mail.com" },
                    new IdentityUser { UserName = "EpicMommy", Email = "mommy@mail.com" },
                    new IdentityUser { UserName = "EpicSonny", Email = "sonny@mail.com" },
                    new IdentityUser { UserName = "EpicSunny", Email = "sunny@mail.com" }
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
                    new IdentityUser { UserName = "StaffJane", Email = "staff1@staff.com" },
                    new IdentityUser { UserName = "StaffJohn", Email = "staff2@staff.com" },
                    new IdentityUser { UserName = "StaffMitch", Email = "staff3@staff.com" }
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

            await _context.StaffMembers.AddRangeAsync(staffMembers);

            //Add companies and customers to the database
            await _context.Companies.AddRangeAsync(companies);

            // Add company representatives to the companies
            companies[0].Representatives.Add(customers[0]);
            companies[0].Representatives.Add(customers[1]);
            companies[1].Representatives.Add(customers[2]);
            companies[1].Representatives.Add(customers[3]);
        }
    }
}
