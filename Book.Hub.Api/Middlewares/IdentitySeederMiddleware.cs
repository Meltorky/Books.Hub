using Books.Hub.Application.Comman;
using Books.Hub.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;

namespace Books.Hub.Api.Middlewares
{
    public static class IdentitySeederMiddleware
    {
        public static async Task IdentitySeeder(this IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUsers.SeedAdminUserAsync(userManager);
        }
    }
}
