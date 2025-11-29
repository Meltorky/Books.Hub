using Books.Hub.Application.Identity;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Books.Hub.Infrastructure.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var superAdminUser = new ApplicationUser
            {
                Email = "superadmin@gmail.com",
                UserName = "@superadmin",
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin"
            };

            var existUser = await userManager.FindByEmailAsync(superAdminUser.Email);

            if (existUser is null)
            {
                await userManager.CreateAsync(superAdminUser, "Ab123456++");
                existUser = superAdminUser;
            }

            List<string> roles = Enum.GetNames(typeof(Roles)).ToList();

            foreach (var role in roles)
                if (!await userManager.IsInRoleAsync(existUser, role))
                    await userManager.AddToRoleAsync(existUser, role);
        }
    }
}
