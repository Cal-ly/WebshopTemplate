using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebshopTemplate.Models;

namespace WebshopTemplate.Areas.Identity.Data.Seeddata
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private UserManager<Customer> _customerManager;
        private UserManager<Staff> _staffManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, UserManager<Customer> customerManager, UserManager<Staff> staffManager)
        {
            _context = context;
            _userManager = userManager;
            _customerManager = customerManager;
            _staffManager = staffManager;
        }

        public async Task SeedDatabase()
        {

            //ArgumentNullException.ThrowIfNull(_userManager);

            //_context.Database.EnsureCreated();

            //var staff1 = new Staff();
            //staff1.UserName = "Staff1";
            //staff1.FirstName = "Staff";
            //staff1.LastName = "One";
            //staff1.Address = "Address 6";
            //staff1.City = "City 6";
            //staff1.PostalCode = "66666";
            //staff1.Country = "Denmark";
            //staff1.Phone = "66666666";
            //staff1.Email = "staff1@staff.com";

            //await _staffManager.CreateAsync(staff1, "1234");
            //await _staffManager.AddToRoleAsync(staff1, "Manager");
            //await _context.SaveChangesAsync();

            //// Define companies
            //var company1 = new Company { Name = "Dead'R'Us", Address = "Street 1", City = "City A", PostalCode = "12345", Country = "Denmark", Email = "contact@companya.com", Phone = "12345678", Website = "www.companya.com" };
            //var company2 = new Company { Name = "FuneralHome", Address = "Street 2", City = "City B", PostalCode = "23456", Country = "Denmark", Email = "contact@companyb.com", Phone = "87654321", Website = "www.companyb.com" };

            //// Define customers

            //Customer customer1 = (Customer)new IdentityUser();
            //customer1 = new Customer { UserName = "JohnDead'R'Us", FirstName = "John", LastName = "Doe", Address = "Address 1", City = "City 1", PostalCode = "11111", Country = "Denmark", Phone = "11111111", Email = "john@mail.com", RepresentingCompany = company1 };

            //Customer customer2 = (Customer)new IdentityUser();
            //customer2 = new Customer { UserName = "JaneDead'R'Us", FirstName = "Jane", LastName = "Doe", Address = "Address 2", City = "City 2", PostalCode = "22222", Country = "Denmark", Phone = "22222222", Email = "jane@mail.com", RepresentingCompany = company1 };

            //Customer customer3 = (Customer)new IdentityUser();
            //customer3 = new Customer { UserName = "FuneralDaddy", FirstName = "Epic", LastName = "CreepyGuy", Address = "Address 3", City = "City 3", PostalCode = "33333", Country = "Denmark", Phone = "33333333", Email = "daddy@mail.com", RepresentingCompany = company2 };

            //Customer customer4 = (Customer)new IdentityUser();
            //customer4 = new Customer { UserName = "FuneralMommy", FirstName = "Epic", LastName = "CreepyGal", Address = "Address 4", City = "City 4", PostalCode = "44444", Country = "Denmark", Phone = "44444444", Email = "mommy@mail.com", RepresentingCompany = company2 };

            //Customer customer5 = (Customer)new IdentityUser();
            //customer5 = new Customer { UserName = "FuneralSonny", FirstName = "Epic", LastName = "CreepyKid", Address = "Address 5", City = "City 5", PostalCode = "55555", Country = "Denmark", Phone = "55555555", Email = "sonny@mail.com" };

            //Customer customer6 = (Customer)new IdentityUser();
            //customer6 = new Customer { UserName = "AlwaysSunny", FirstName = "Flower", LastName = "Power", Address = "Address 3", City = "City 3", PostalCode = "33333", Country = "Denmark", Phone = "33333333", Email = "sunny@mail.com" };

            //Customer[] customerArray = { customer1, customer2, customer3, customer4, customer5 };

            //// Define Staff
            //Staff Staff1 = new Staff { UserName = "Staff1", FirstName = "Staff", LastName = "One", Address = "Address 6", City = "City 6", PostalCode = "66666", Country = "Denmark", Phone = "66666666", Email = "staff1@staff.com" };
            //Staff Staff2 = new Staff { UserName = "Staff2", FirstName = "Staff", LastName = "Two", Address = "Address 7", City = "City 7", PostalCode = "77777", Country = "Denmark", Phone = "77777777", Email = "staff2@staff.com" };
            //Staff Staff3 = new Staff { UserName = "Staff3", FirstName = "Staff", LastName = "Three", Address = "Address 8", City = "City 8", PostalCode = "88888", Country = "Denmark", Phone = "88888888", Email = "staff3@staff.com" };

            //Staff[] staffArray = { Staff1, Staff2, Staff3 };

            ////Add companies and customers to the database
            //context.Companies.Add(company1);
            //context.Companies.Add(company2);

            //// Define IdentityUser password and roles
            //string password = "Password123!";
            //string managerRole = "Manager";
            //string superMemberRole = "SuperMember";
            //string memberRole = "Member";


            //// Add roles to the database
            //foreach (var c in customerArray)
            //{
            //    if (await userManager.FindByEmailAsync(c.Email) == null)
            //    {
            //        await userManager.CreateAsync(c, password);
            //    }
            //    await userManager.AddToRoleAsync(c, "SuperMember");
            //    //context.Customers.Add(c);
            //}

            //foreach (var s in staffArray)
            //{
            //    if (await userManager.FindByEmailAsync(s.Email) == null)
            //    {
            //        await userManager.CreateAsync(s, password);
            //    }
            //    await userManager.AddToRoleAsync(s, "Manager");
            //    //context.StaffMembers.Add(s);
            //}

            //await userManager.AddPasswordAsync(customer6, password);
            //await userManager.AddToRoleAsync(customer6, "Member");
            //context.Customers.Add(customer6);

            //// Add company representatives to the companies
            //company1.Representatives.Add(customer1);
            //company1.Representatives.Add(customer2);
            //company2.Representatives.Add(customer3);
            //company2.Representatives.Add(customer4);

            //// Save changes to the database
            //await context.SaveChangesAsync();
        }
    }
}
