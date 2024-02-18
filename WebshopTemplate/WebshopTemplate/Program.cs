global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.EntityFrameworkCore.Design;
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
global using System.Threading.Tasks;
global using WebshopTemplate.Data;
global using WebshopTemplate.Models;
global using WebshopTemplate.Areas.Identity.Data;
global using WebshopTemplate.Areas.Identity.Data.Seeddata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;

namespace WebshopTemplate;
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityCore<Customer>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddRoles<IdentityRole>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<Customer>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityCore<Staff>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var context =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager =
            //    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //var customerManager =
            //    scope.ServiceProvider.GetRequiredService<UserManager<Customer>>();
            //var staffManager =
            //    scope.ServiceProvider.GetRequiredService<UserManager<Staff>>();
            //var logger =
            //    scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            // EnsureDeleted() and EnsureCreated() are used to create a new database and seed it with date
            // In this case roles and a later an admin user.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var roles = new[] { "Admin", "Manager", "SuperMember", "Member" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager =
                scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var customerManager =
                scope.ServiceProvider.GetRequiredService<UserManager<Customer>>();
            var staffManager =
                scope.ServiceProvider.GetRequiredService<UserManager<Staff>>();
            var context =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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

            string managerRole = "Manager";
            string managerEmail = "manager@manager.com";
            string managerPassword = "1234";

            var manager = await userManager.FindByEmailAsync(managerEmail.Normalize());

            if (await userManager.FindByEmailAsync(managerEmail) == null)
            {
                manager = new IdentityUser();
                manager.UserName = managerEmail;
                manager.Email = managerEmail;
                manager.EmailConfirmed = true;
                await userManager.CreateAsync(manager, managerPassword);
                await userManager.AddToRoleAsync(manager, managerRole);
            }

            var staff1 = new Staff();
            staff1.UserName = "Staff";
            staff1.FirstName = "Staff";
            staff1.LastName = "One";
            staff1.Address = "Address 6";
            staff1.City = "City 6";
            staff1.PostalCode = "66666";
            staff1.Country = "Denmark";
            staff1.Phone = "66666666";
            staff1.Email = "staff1@staff.com";

            await userManager.CreateAsync(staff1, "1234");
            await userManager.AddToRoleAsync(staff1, "Manager");
            //DbInitializer DatabaseInitializer = new DbInitializer(context, userManager, customerManager, staffManager);
            //await DatabaseInitializer.SeedDatabase();
        }
        app.Run();
    }
}

// out commented code

//AddIdentityServices(builder);
//AddAuthorizationPolicies(builder);
//ConfigureApplicationCookie(builder);

//static void AddIdentityServices(WebApplicationBuilder builder)
//{
//    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//        .AddRoles<IdentityRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>();
//}
//static void AddAuthorizationPolicies(WebApplicationBuilder builder)
//{
//    builder.Services.AddAuthorization(options =>
//    {
//        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//        options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Admin", "Manager"));
//        options.AddPolicy("RequireSuperMemberRole", policy => policy.RequireRole("Admin", "Manager", "SuperMember"));
//        options.AddPolicy("RequireMemberRole", policy => policy.RequireRole("Admin", "Manager", "SuperMember", "Member"));
//        options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
//    });
//}
//static void ConfigureApplicationCookie(WebApplicationBuilder builder)
//{
//    builder.Services.ConfigureApplicationCookie(options =>
//    {
//        options.Cookie.Name = "WebshopTemplateCookie";
//        options.LoginPath = "/Identity/Account/Login";
//        options.LogoutPath = "/Identity/Account/Logout";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
//        options.SlidingExpiration = true;
//    });

//    builder.Services.Configure<CookiePolicyOptions>(options =>
//    {
//        options.CheckConsentNeeded = context => true;
//        options.MinimumSameSitePolicy = SameSiteMode.None;
//        options.ConsentCookie.Name = "WebshopTemplateConsentCookie";
//        options.ConsentCookie.Expiration = TimeSpan.FromDays(365);
//    });
//}
