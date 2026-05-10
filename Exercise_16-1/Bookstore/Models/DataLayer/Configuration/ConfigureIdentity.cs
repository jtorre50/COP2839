using Microsoft.AspNetCore.Identity;

namespace Bookstore.Models
{
    public class ConfigureIdentity
    {
        public static async Task CreateAdminUserAsync(IServiceProvider services)
        {
            RoleManager<IdentityRole> roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<User> userManager =
                services.GetRequiredService<UserManager<User>>();

            string username = "admin";
            string password = "Sesame";
            string roleName = "Admin";

            // create role if it doesn't exist
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // create user if it doesn't exist
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
