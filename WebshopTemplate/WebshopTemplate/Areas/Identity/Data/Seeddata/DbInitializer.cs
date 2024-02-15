using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebshopTemplate.Areas.Identity.Data.Seeddata
{
    public static class DbInitializer
    {
        public static async Task Initializer(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
