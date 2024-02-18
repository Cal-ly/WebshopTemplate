﻿using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebshopTemplate.Models;

namespace WebshopTemplate.Areas.Identity.Data.Seeddata
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
            var company1 = new Company { Name = "Dead'R'Us", Address = "Street 1", City = "City A", PostalCode = "12345", Country = "Denmark", Email = "contact@companya.com", Phone = "12345678", Website = "www.companya.com" };
            var company2 = new Company { Name = "FuneralHome", Address = "Street 2", City = "City B", PostalCode = "23456", Country = "Denmark", Email = "contact@companyb.com", Phone = "87654321", Website = "www.companyb.com" };

            //// Define customers

            IdentityUser customerUser1 = new IdentityUser { UserName = "JohnDead", Email = "john@mail.com" };
            var result1 = await _userManager.CreateAsync(customerUser1, password);
            if (result1.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser1, superMemberRole);
            }
            Customer customer1 = new Customer { UserId = customerUser1.Id, User = customerUser1, FirstName = "John", LastName = "Doe", Address = "Address 1", City = "City 1", PostalCode = "11111", Country = "Denmark", Phone = "11111111", RepresentingCompany = company1 };

            IdentityUser customerUser2 = new IdentityUser { UserName = "JaneDead", Email = "jane@mail.com" };
            var result2 = await _userManager.CreateAsync(customerUser2, password);
            if (result2.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser2, superMemberRole);
            }
            Customer customer2 = new Customer { UserId = customerUser2.Id, User = customerUser2, FirstName = "Jane", LastName = "Doe", Address = "Address 2", City = "City 2", PostalCode = "22222", Country = "Denmark", Phone = "22222222", RepresentingCompany = company1 };

            IdentityUser customerUser3 = new IdentityUser { UserName = "Epic", Email = "daddy@mail.com" };
            var result3 = await _userManager.CreateAsync(customerUser3, password);
            if (result3.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser3, superMemberRole);
            }
            Customer customer3 = new Customer { UserId = customerUser3.Id, User = customerUser3, FirstName = "Epic", LastName = "CreepyGuy", Address = "Address 3", City = "City 3", PostalCode = "33333", Country = "Denmark", Phone = "33333333", RepresentingCompany = company2 };

            IdentityUser customerUser4 = new IdentityUser { UserName = "Epic", Email = "mommy@mail.com" };
            var result4 = await _userManager.CreateAsync(customerUser4, password);
            if (result4.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser4, superMemberRole);
            }
            Customer customer4 = new Customer { UserId = customerUser4.Id, User = customerUser4, FirstName = "Epic", LastName = "CreepyGal", Address = "Address 4", City = "City 4", PostalCode = "44444", Country = "Denmark", Phone = "44444444", RepresentingCompany = company2 };

            IdentityUser customerUser5 = new IdentityUser { UserName = "Epic", Email = "sonny@mail.com" };
            var result5 = await _userManager.CreateAsync(customerUser5, password);
            if (result5.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser5, superMemberRole);
            }
            Customer customer5 = new Customer { UserId = customerUser5.Id, User = customerUser5, FirstName = "Epic", LastName = "CreepyKid", Address = "Address 5", City = "City 5", PostalCode = "55555", Country = "Denmark", Phone = "55555555" };

            IdentityUser customerUser6 = new IdentityUser { UserName = "Epic", Email = "sunny@mail.com" };
            var result6 = await _userManager.CreateAsync(customerUser6, password);
            if (result6.Succeeded)
            {
                await _userManager.AddToRoleAsync(customerUser6, memberRole);
            }
            Customer customer6 = new Customer { UserId = customerUser6.Id, User = customerUser6, FirstName = "Flower", LastName = "Power", Address = "Address 3", City = "City 3", PostalCode = "33333", Country = "Denmark", Phone = "33333333" };

            Customer[] customerArray = { customer1, customer2, customer3, customer4, customer5, customer6 };

            await _context.Customers.AddRangeAsync(customerArray);

            // Define Staff
            IdentityUser staffUser1 = new IdentityUser { UserName = "StaffJane", Email = "staff1@staff.com" };
            var result7 = await _userManager.CreateAsync(staffUser1, password);
            if (result7.Succeeded)
            {
                await _userManager.AddToRoleAsync(staffUser1, managerRole);
            }
            Staff Staff1 = new Staff { UserId = staffUser1.Id, User = staffUser1, FirstName = "StaffJane", LastName = "One", Address = "Address 6", City = "City 6", PostalCode = "66666", Country = "Denmark", Phone = "66666666" };

            IdentityUser staffUser2 = new IdentityUser { UserName = "StaffJohn", Email = "staff2@staff.com" };
            var result8 = await _userManager.CreateAsync(staffUser2, password);
            if (result8.Succeeded)
            {
                await _userManager.AddToRoleAsync(staffUser2, managerRole);
            }
            Staff Staff2 = new Staff { UserId = staffUser2.Id, User = staffUser2, FirstName = "StaffJohn", LastName = "Two", Address = "Address 7", City = "City 7", PostalCode = "77777", Country = "Denmark", Phone = "77777777" };

            IdentityUser staffUser3 = new IdentityUser { UserName = "StaffMitch", Email = "staff3@staff.com" };
            var result9 = await _userManager.CreateAsync(staffUser3, password);
            if (result9.Succeeded)
            {
                await _userManager.AddToRoleAsync(staffUser3, managerRole);
            }
            Staff Staff3 = new Staff { UserId = staffUser3.Id, User = staffUser3, FirstName = "StaffMitch", LastName = "Three", Address = "Address 8", City = "City 8", PostalCode = "88888", Country = "Denmark", Phone = "88888888" };

            Staff[] staffArray = { Staff1, Staff2, Staff3 };

            await _context.StaffMembers.AddRangeAsync(staffArray);

            //Add companies and customers to the database
            _context.Companies.Add(company1);
            _context.Companies.Add(company2);

            // Add company representatives to the companies
            company1.Representatives.Add(customer1);
            company1.Representatives.Add(customer2);
            company2.Representatives.Add(customer3);
            company2.Representatives.Add(customer4);

        }
    }
}
