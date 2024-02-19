global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Builder.Extensions;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Mvc.ViewFeatures;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Routing.Template;
global using Microsoft.AspNetCore.StaticFiles;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
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
global using WebshopTemplate.Seeddata;
global using WebshopTemplate.Models;
global using WebshopTemplate.Interfaces;
global using WebshopTemplate.Repositories;
global using WebshopTemplate.Services;
global using WebshopTemplate.DTO;
global using WebshopTemplate.Pages.Analytics;
global using WebshopTemplate.Helpers;

namespace WebshopTemplate;
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });



        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();


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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var context =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
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

            DbInitializer DatabaseInitializer = new DbInitializer(context, userManager);
            await DatabaseInitializer.SeedUserDatabase();
        }
        app.Run();
    }
}