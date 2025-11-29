using Books.Hub.Domain.Constants;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager) 
        {
            List<string> roles = Enum.GetNames(typeof(Roles)).ToList();

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole { Name = role});
        }
    }
}
