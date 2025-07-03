using Books.Hub.Application.Identity;
using Books.Hub.Domain.Constants;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Books.Hub.Infrastructure.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager) 
        {
            var adminUser = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                EmailConfirmed = true,               
                FirstName = "Admin",
                LastName = "Admin"
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) is null)
            {
                string password = "Ab123456+";
                var result = await userManager.CreateAsync(adminUser,password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser,Roles.Admin.ToString());
                }
            }
        }
    }
}
