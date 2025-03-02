using Books.Hub.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager) 
        {
            if (!await roleManager.Roles.AnyAsync()) 
            {
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString()});
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.Author.ToString()});
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.Subscriber.ToString()});
            }
        }
    }
}
