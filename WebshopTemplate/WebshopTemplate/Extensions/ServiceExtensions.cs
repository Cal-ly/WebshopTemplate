namespace WebshopTemplate;

public static class ServiceExtensions
{
    public static IServiceCollection Config(this IServiceCollection services)
    {

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.Name = ".WebshopTemplate.Session";
            // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.IOTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.SameSite = SameSiteMode.None; //.Strict; should be strict, but strict is not supported by all browsers and we are in a development environment

        });

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            // User settings
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            // If the LoginPath isn't set, ASP.NET Core defaults the path to /Account/Login.
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });
        return services;
    }
}