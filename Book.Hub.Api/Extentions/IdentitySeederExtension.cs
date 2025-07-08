using Books.Hub.Infrastructure.Seeds;

public static class IdentitySeederExtension
{
    public static async Task SeedIdentityAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await DefaultRoles.SeedAsync(roleManager);
        await DefaultUsers.SeedAdminUserAsync(userManager);
    }
}
