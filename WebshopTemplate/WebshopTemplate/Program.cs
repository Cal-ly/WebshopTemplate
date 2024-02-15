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

namespace WebshopTemplate;
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
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
            var logger =
                scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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

            string adminRole = "Admin";
            string adminEmail = "admin@admin.com";
            string adminPassword = "Admin1234!";

            var user = await userManager.FindByEmailAsync(adminEmail.Normalize());

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                user = new IdentityUser();
                user.UserName = adminEmail;
                user.Email = adminEmail;
                user.EmailConfirmed = true;

                await userManager.CreateAsync(user, adminPassword);

                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
        app.Run();
    }
}





// out commented code

//AddIdentityServices(builder);
//AddAuthorizationPolicies(builder);
//ConfigureApplicationCookie(builder);

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

//// app.UseEndpoints(endpoints => endpoints.MapRazorPages());

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();
//app.UseAuthentication();
//app.MapRazorPages();

//EnsureDatabaseIntegrity(app).Wait();
//EnsureRolesAndAdminUser(app).Wait();

//app.Run();

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
//static Task EnsureDatabaseIntegrity(WebApplication app)
//{
//    using var scope = app.Services.CreateScope();
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<ApplicationDbContext>();
//    var identityContext = services.GetRequiredService<IdentityDbContext>();
//    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var logger = services.GetRequiredService<ILogger<Program>>();

//    if (app.Environment.IsDevelopment())
//    {
//        app.UseDeveloperExceptionPage();
//    }
//    else
//    {
//        app.UseExceptionHandler("/Error");
//        app.UseHsts();
//    }

//    DbInitializer.Initialize(context, userManager, roleManager).Wait();
//    return Task.CompletedTask;
//}
//static async Task EnsureRolesAndAdminUser(WebApplication app)
//{
//    using var scope = app.Services.CreateScope();
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<ApplicationDbContext>();
//    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    await DbInitializer.Initialize(context, userManager, roleManager);
//}