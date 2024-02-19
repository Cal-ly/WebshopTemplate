namespace WebshopTemplate.Initializers
{
    public static class AdminInitializer
    {
        public static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string adminRole = "Admin";
            string adminEmail = "admin@admin.com";
            string adminPassword = "1234";

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
    }
}
