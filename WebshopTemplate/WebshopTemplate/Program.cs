global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics;
global using System.Linq;
global using System.Security.Claims;
global using System.Threading.Tasks;

using WebshopTemplate.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

app.MapRazorPages();
app.Run();

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

    if (app.Environment.IsDevelopment())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
    else
    {
        context.Database.Migrate();
    }

    string email = "admin@admin.com";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(adminUser, "Admin1234!");

        await userManager.AddToRoleAsync(adminUser, "Admin");
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
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

//    string email = "admin@admin.com";

//    if (await userManager.FindByEmailAsync(email) == null)
//    {
//        var adminUser = new IdentityUser
//        {
//            UserName = email,
//            Email = email,
//            EmailConfirmed = true,
//        };

//        await userManager.CreateAsync(adminUser, "Admin1234!");

//        await userManager.AddToRoleAsync(adminUser, "Admin");
//    }
//}