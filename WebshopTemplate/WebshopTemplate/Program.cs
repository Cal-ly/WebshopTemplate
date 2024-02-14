global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using System.ComponentModel.DataAnnotations.Schema;
using WebshopTemplate.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints => endpoints.MapRazorPages()); // Map Razor Pages to the endpoints.. This is the default route for Razor Pages, so it is not necessary to add a route for Razor Pages.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

/// <summary>
/// This scope is used to create roles if they do not exist.
/// </summary>
/// <comment>
/// Roles are created here to ensure that they exist in the database. This is useful when deploying the application to a new environment.
/// Be aware that this code will run every time the application starts, so it is important to check if the roles exist before creating them.
/// A fix to this would be to create a migration that adds the roles to the database, and then run the migration when deploying the application.
/// However in development, this is a quick and easy way to ensure that the roles exist in the database.
/// Change this code, when deploying to a production environment, to use a more secure method of creating roles.
/// </comment>
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; // Get the services from the service provider, added for encapsulation
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    var roles = new[] { "Admin", "Manager", "SuperMember", "Member" };  // Add roles here

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

/// <summary>
/// Creates a default admin user if one does not exist.
/// </summary>
/// <comment>
/// Be sure to change the email and password to something more secure in a production environment.
/// A fix to this would be to create a migration that adds the admin user to the database, and then run the migration when deploying the application.
/// Alternatively, you could create a registration page for the admin user to register themselves.
/// Hardcoding the admin user's email and password is not secure, and should be avoided in a production environment.
/// However in development, this is a quick and easy way to ensure that the admin user exists in the database.
/// </comment>
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, "Admin1234!");

        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();