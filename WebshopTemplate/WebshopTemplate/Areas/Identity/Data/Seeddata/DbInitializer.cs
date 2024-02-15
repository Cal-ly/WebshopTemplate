using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebshopTemplate.Areas.Identity.Data.Seeddata
{
    public static class DbInitializer
    {
        private static readonly string[] adminRoles = ["Admin"];

        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            string[] roleNames = { "Admin", "Manager", "SuperMember", "Member" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = await userManager.FindByEmailAsync("admin@admin.com");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    //FirstName = "Admin",
                    //LastName = "Admin",
                    //Address = "Admin Street 1",
                    //City = "Admin City",
                    //PostalCode = "1234",
                    //Country = "Denmark",
                    //Phone = "12345678",
                    //EmploymentDate = DateTime.UtcNow,
                    //BasePay = 0
                };

                await userManager.CreateAsync(user, "Admin1234!");

                await userManager.AddToRolesAsync(user, adminRoles);
            }
        }
    }
}
