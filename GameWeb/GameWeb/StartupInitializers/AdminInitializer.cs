using GameWeb.Entities;
using Microsoft.AspNetCore.Identity;

namespace MathWars.StartupInitializers;

public static class AdminInitializer
{
	public static async Task InitializeUserAsync(UserManager<ApplicationUsers> userManager, IConfiguration configuration)
	{
		var adminData = configuration.GetSection("DefaultAdminData");

		var adminUserName = adminData["UserName"];

		var adminUser = await userManager.FindByNameAsync(adminUserName);

		if (adminUser == null)
		{
			var adminEmail = adminData["Email"];
			var adminPassword = adminData["Password"];
			var ApplicationRoles = configuration.GetSection("ApplicationRoles");
			var roleName = ApplicationRoles["Admin"];
			adminUser = new ApplicationUsers { UserName = adminUserName, Email = adminEmail };
			var result = await userManager.CreateAsync(adminUser, adminPassword);

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(adminUser, roleName);
			}
			else
			{
				throw new Exception($"Error creating user '{adminUserName}'.");
			}
		}
	}
}
