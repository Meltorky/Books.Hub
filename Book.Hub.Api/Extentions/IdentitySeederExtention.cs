using Books.Hub.Application.Comman;
using Books.Hub.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;

namespace Books.Hub.Api.Extentions
{
    public static class IdentitySeederExtention
    {
        public static async Task IdentitySeeder(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUsers.SeedAdminUserAsync(userManager);
        }
    }
}
